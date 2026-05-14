using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class DoctorService : GenericService<Doctor>
{
    private readonly DoctorRepository _doctorRepository;
    private readonly Mapper _mapper;
    private readonly UserRepository _userRepository;

    public DoctorService(DoctorRepository doctorRepository, UserRepository userRepository) : base(doctorRepository)
    {
        _doctorRepository = doctorRepository;
        _userRepository = userRepository;
        _mapper = MapperConfig.GetMapper();
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        if (doctor == null) return null;
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentAsync(int departmentId)
    {
        var doctors = await _doctorRepository.GetDoctorsByDepartmentAsync(departmentId);
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<bool> AddDoctorAsync(DoctorDto dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing == null) return false;

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.RoleId = 2;
        await _userRepository.AddAsync(user);

        var doctor = _mapper.Map<Doctor>(dto);
        doctor.UserId = user.Id;
        doctor.IsAvailable = true;
        await _doctorRepository.AddAsync(doctor);

        return true;
    }

    public async Task<bool> UpdateDoctorAsync(DoctorDto dto)
    {
        var doctor = await _doctorRepository.GetDoctorWithUserAsync(dto.Id);
        if (doctor == null) return false;

        doctor.User.Name = dto.Name;
        doctor.User.Gender = dto.Gender;
        doctor.User.PhoneNumber = dto.PhoneNumber;
        doctor.User.Address = dto.PhoneNumber;
        doctor.User.DateOfBirth = dto.DateOfBirth;
        await _userRepository.UpdateAsync(doctor.User);

        doctor.Specialty = dto.Specity;
        doctor.LicenseNumber = dto.LicenseNumber;
        doctor.Fee = dto.Fee;
        doctor.DepartmentId = dto.DepartmentId;
        doctor.IsAvailable = dto.IsAvailable;
        await _doctorRepository.UpdateAsync(doctor);

        return true;
    }

    public async Task<bool> DeactivateDoctorAsync(int id)
    {
        var doctor = await _doctorRepository.GetDoctorWithUserAsync(id);
        if (doctor == null) return false;

        doctor.User.IsActive = false;
        await _userRepository.UpdateAsync(doctor.User);

        return true;
    }
}
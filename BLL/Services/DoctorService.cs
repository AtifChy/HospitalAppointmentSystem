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

    public IEnumerable<DoctorDto> GetAllDoctors()
    {
        var doctors = _doctorRepository.GetAll();
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public DoctorDto? GetDoctorById(int id)
    {
        var doctor = _doctorRepository.GetById(id);
        if (doctor == null) return null;
        return _mapper.Map<DoctorDto>(doctor);
    }

    public IEnumerable<DoctorDto> GetDoctorsByDepartment(int departmentId)
    {
        var doctors = _doctorRepository.GetDoctorsByDepartment(departmentId);
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public bool AddDoctor(DoctorDto dto)
    {
        var existing = _userRepository.GetByEmail(dto.Email);
        if (existing == null) return false;

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.RoleId = 2;
        _userRepository.Add(user);

        var doctor = _mapper.Map<Doctor>(dto);
        doctor.UserId = user.Id;
        doctor.IsAvailable = true;
        _doctorRepository.Add(doctor);

        return true;
    }

    public bool UpdateDoctor(DoctorDto dto)
    {
        var doctor = _doctorRepository.GetDoctorWithUser(dto.Id);
        if (doctor == null) return false;

        doctor.User.Name = dto.Name;
        doctor.User.Gender = dto.Gender;
        doctor.User.PhoneNumber = dto.PhoneNumber;
        doctor.User.Address = dto.PhoneNumber;
        doctor.User.DateOfBirth = dto.DateOfBirth;
        _userRepository.Update(doctor.User);

        doctor.Specialty = dto.Specity;
        doctor.LicenseNumber = dto.LicenseNumber;
        doctor.Fee = dto.Fee;
        doctor.DepartmentId = dto.DepartmentId;
        doctor.IsAvailable = dto.IsAvailable;
        _doctorRepository.Update(doctor);

        return true;
    }

    public bool DeactivateDoctor(int id)
    {
        var doctor = _doctorRepository.GetDoctorWithUser(id);
        if (doctor == null) return false;

        doctor.User.IsActive = false;
        _userRepository.Update(doctor.User);

        return true;
    }
}
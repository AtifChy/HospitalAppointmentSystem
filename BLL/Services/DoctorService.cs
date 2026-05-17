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

    public List<DoctorDto> GetAllDoctors()
    {
        var doctors = _doctorRepository.GetDoctorsAll();
        return _mapper.Map<List<DoctorDto>>(doctors);
    }

    public DoctorDto? GetDoctorByUserId(int id)
    {
        var doctor = _doctorRepository.GetDoctorByUserId(id);
        if (doctor == null) return null;
        return _mapper.Map<DoctorDto>(doctor);
    }

    public DoctorDto? GetDoctorById(int id)
    {
        var doctor = _doctorRepository.GetDoctorById(id);
        if (doctor == null) return null;
        return _mapper.Map<DoctorDto>(doctor);
    }

    public List<DoctorDto> GetDoctorsByDepartmentId(int departmentId)
    {
        var doctors = _doctorRepository.GetDoctorsByDepartmentId(departmentId);
        return _mapper.Map<List<DoctorDto>>(doctors);
    }

    public bool AddDoctor(DoctorDto dto)
    {
        var existing = _userRepository.GetByEmail(dto.Email);
        if (existing != null) return false;

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.RoleId = 2;
        user.MustChangePassword = true;
        _userRepository.Add(user);

        var doctor = _mapper.Map<Doctor>(dto);
        doctor.UserId = user.Id;
        doctor.IsAvailable = true;
        _doctorRepository.Add(doctor);

        return true;
    }

    public bool UpdateDoctor(DoctorDto dto)
    {
        var doctor = _doctorRepository.GetDoctorById(dto.Id);
        if (doctor == null) return false;

        // update user properties
        _mapper.Map(dto, doctor.User);
        // update doctor properties
        _mapper.Map(dto, doctor);

        _userRepository.Update(doctor.User);
        _doctorRepository.Update(doctor);

        return true;
    }

    public bool DeactivateDoctor(int id)
    {
        var doctor = _doctorRepository.GetDoctorById(id);
        if (doctor == null) return false;

        doctor.User.IsActive = false;
        _userRepository.Update(doctor.User);

        return true;
    }

    public bool MarkForPasswordChange(int id)
    {
        var doctor = _doctorRepository.GetDoctorById(id);
        if (doctor == null) return false;

        doctor.User.MustChangePassword = true;
        _userRepository.Update(doctor.User);

        return true;
    }
}
using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class AuthService
{
    private readonly Mapper _mapper;
    private readonly PatientRepository _patientRepository;
    private readonly UserRepository _userRepository;

    public AuthService(PatientRepository patientRepository, UserRepository userRepository)
    {
        _patientRepository = patientRepository;
        _userRepository = userRepository;
        _mapper = MapperConfig.GetMapper();
    }

    public UserDto? Login(LoginDto dto)
    {
        var user = _userRepository.GetByEmail(dto.Email);
        if (user == null) return null;
        if (!user.IsActive) return null;
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;
        return _mapper.Map<UserDto>(user);
    }

    public bool Register(RegisterDto dto)
    {
        var existing = _userRepository.GetByEmail(dto.Email);
        if (existing != null) return false;

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.RoleId = 3;
        _userRepository.Add(user);

        var patient = _mapper.Map<Patient>(dto);
        _patientRepository.Add(patient);

        return true;
    }

    public bool ChangePassword(int userId, string oldPassword, string newPassword)
    {
        var user = _userRepository.GetById(userId);
        if (user == null) return false;
        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash)) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.MustChangePassword = false;
        _userRepository.Update(user);

        return true;
    }
}
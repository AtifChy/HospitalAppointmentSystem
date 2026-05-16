using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class AdminService : GenericService<User>
{
    private readonly UserRepository _userRepository;
    private readonly Mapper _mapper;

    public AdminService(UserRepository userRepository) : base(userRepository)
    {
        _userRepository = userRepository;
        _mapper = MapperConfig.GetMapper();
    }

    public List<AdminDto> GetAllAdmins()
    {
        // RoleId 1 is Admin
        var admins = _userRepository.GetAll().Where(u => u.RoleId == 1).ToList();
        return _mapper.Map<List<AdminDto>>(admins);
    }

    public AdminDto? GetAdminById(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null || user.RoleId != 1) return null;
        return _mapper.Map<AdminDto>(user);
    }

    public bool AddAdmin(AdminDto dto)
    {
        var existing = _userRepository.GetByEmail(dto.Email);
        if (existing != null) return false;

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password ?? "Admin123");
        user.RoleId = 1; // Admin
        user.MustChangePassword = true;
        user.IsActive = true;
        _userRepository.Add(user);

        return true;
    }

    public bool UpdateAdmin(AdminDto dto)
    {
        var user = _userRepository.GetById(dto.Id);
        if (user == null || user.RoleId != 1) return false;

        _mapper.Map(dto, user);
        
        // Don't update password here unless provided
        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        _userRepository.Update(user);
        return true;
    }
}

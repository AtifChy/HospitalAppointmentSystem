using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class DepartmentService : GenericService<Department>
{
    private readonly DepartmentRepository _departmentRepository;
    private readonly Mapper _mapper;

    public DepartmentService(DepartmentRepository departmentRepository) : base(departmentRepository)
    {
        _departmentRepository = departmentRepository;
        _mapper = MapperConfig.GetMapper();
    }

    public List<DepartmentDto> GetAllDepartments()
    {
        var departments = _departmentRepository.GetAll();
        return _mapper.Map<List<DepartmentDto>>(departments);
    }

    public DepartmentDto? GetDepartmentById(int id)
    {
        var department = _departmentRepository.GetById(id);
        if (department == null) return null;
        return _mapper.Map<DepartmentDto>(department);
    }

    public bool AddDepartment(DepartmentDto dto)
    {
        var existing = _departmentRepository.GetDepartmentByName(dto.Name);
        if (existing != null) return false;

        var department = _mapper.Map<Department>(dto);
        _departmentRepository.Add(department);
        return true;
    }

    public bool UpdateDepartment(DepartmentDto dto)
    {
        var department = _departmentRepository.GetById(dto.Id);
        if (department == null) return false;

        _mapper.Map(dto, department);
        _departmentRepository.Update(department);
        return true;
    }
}
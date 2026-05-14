using DAL.Repositories;

namespace BLL.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _departmentRepository;

    public DepartmentService(DepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }
}
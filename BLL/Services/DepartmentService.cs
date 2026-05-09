using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class DepartmentService : GenericService<Department>
{
    public DepartmentService(DepartmentRepository repository) : base(repository)
    {
    }
}
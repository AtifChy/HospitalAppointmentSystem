using DAL.Context;
using DAL.Models;

namespace DAL.Repositories;

public class DepartmentRepository : GenericRepository<Department>
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    public Department? GetDepartmentByName(string name)
    {
        return _context.Departments.FirstOrDefault(d => d.Name == name);
    }
}
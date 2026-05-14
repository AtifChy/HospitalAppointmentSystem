using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class DoctorRepository : GenericRepository<Doctor>
{
    public DoctorRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Doctor> GetDoctorsByDepartment(int departmentId)
    {
        return _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .Where(d => d.DepartmentId == departmentId)
            .ToList();
    }

    public Doctor? GetDoctorWithUser(int id)
    {
        return _context.Doctors
            .Include(d => d.User)
            .FirstOrDefault(d => d.Id == id);
    }
}
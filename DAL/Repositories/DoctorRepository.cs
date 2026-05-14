using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class DoctorRepository : GenericRepository<Doctor>
{
    public DoctorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId)
    {
        return await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .Where(d => d.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<Doctor?> GetDoctorWithUserAsync(int id)
    {
        return await _context.Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}
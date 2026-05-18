using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class DoctorRepository : GenericRepository<Doctor>
{
    public DoctorRepository(AppDbContext context) : base(context)
    {
    }

    public List<Doctor> GetActiveDoctorsByDepartmentId(int departmentId)
    {
        return _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .Where(d => d.IsAvailable == true)
            .Where(d => d.DepartmentId == departmentId)
            .ToList();
    }

    public List<Doctor> GetDoctorsAll()
    {
        return _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .ToList();
    }

    public Doctor? GetDoctorById(int id)
    {
        return _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstOrDefault(d => d.Id == id);
    }

    public Doctor? GetDoctorByUserId(int id)
    {
        return _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstOrDefault(d => d.User.Id == id);
    }

    public bool IsSlotTaken(int doctorId, DateTime date, TimeSpan timeSlot)
    {
        return _context.Appointments
            .Any(a => a.DoctorId == doctorId && a.Date == date && a.TimeSlot == timeSlot);
    }
}
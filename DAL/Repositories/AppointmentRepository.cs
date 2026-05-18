using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>
{
    public AppointmentRepository(AppDbContext context) : base(context)
    {
    }

    public List<Appointment> GetRecentAppointments(int count)
    {
        return _context.Appointments
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Department)
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToList();
    }

    public Appointment? GetAppointmentWithDetails(int id)
    {
        return _context.Appointments
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Prescription)
            .FirstOrDefault(a => a.Id == id);
    }

    public List<Appointment> GetByDoctorId(int doctorId)
    {
        return _context.Appointments
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Department)
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Where(a => a.DoctorId == doctorId)
            .ToList();
    }

    public List<Appointment> GetByPatientId(int patientId)
    {
        return _context.Appointments
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Department)
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Where(a => a.PatientId == patientId)
            .ToList();
    }
}
using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PatientRepository : GenericRepository<Patient>
{
    public PatientRepository(AppDbContext context) : base(context)
    {
    }

    public List<Patient> GetPatientsAll()
    {
        return _context.Patients
            .Include(p => p.User)
            .ToList();
    }

    public Patient? GetPatientById(int id)
    {
        return _context.Patients
            .Include(p => p.User)
            .FirstOrDefault(p => p.Id == id);
    }
}
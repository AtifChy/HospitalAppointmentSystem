using DAL.Context;
using DAL.Models;

namespace DAL.Repositories;

public class PatientRepository : GenericRepository<Patient>
{
    public PatientRepository(AppDbContext context) : base(context)
    {
    }
}
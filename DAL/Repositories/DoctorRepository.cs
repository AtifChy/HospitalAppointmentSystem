using DAL.Context;
using DAL.Models;

namespace DAL.Repositories;

public class DoctorRepository : GenericRepository<Doctor>
{
    public DoctorRepository(AppDbContext context) : base(context)
    {
    }
}
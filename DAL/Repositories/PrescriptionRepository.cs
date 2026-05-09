using DAL.Context;
using DAL.Models;

namespace DAL.Repositories;

public class PrescriptionRepository : GenericRepository<Prescription>
{
    public PrescriptionRepository(AppDbContext context) : base(context)
    {
    }
}
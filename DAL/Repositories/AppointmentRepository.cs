using DAL.Context;
using DAL.Models;

namespace DAL.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>
{
    public AppointmentRepository(AppDbContext context) : base(context)
    {
    }
}
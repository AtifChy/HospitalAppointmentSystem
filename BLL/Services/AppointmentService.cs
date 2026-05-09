using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class AppointmentService : GenericService<Appointment>
{
    public AppointmentService(AppointmentRepository repository) : base(repository)
    {
    }
}
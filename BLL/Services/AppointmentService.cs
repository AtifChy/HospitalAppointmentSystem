using DAL.Repositories;

namespace BLL.Services;

public class AppointmentService
{
    private readonly AppointmentRepository _appointmentRepository;

    public AppointmentService(AppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
}
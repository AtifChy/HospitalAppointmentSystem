using BLL.DTOs;

namespace Web.Models;

public class PatientDashboardViewModel
{
    public int TotalAppointments { get; set; }
    public int PendingAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public List<AppointmentDto> UpcomingAppointments { get; set; } = new();
}
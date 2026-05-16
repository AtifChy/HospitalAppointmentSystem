using BLL.DTOs;

namespace Web.Models;

public class DoctorDashboardViewModel
{
    public int TotalAppointments { get; set; }
    public int PendingAppointments { get; set; }
    public int ConfirmedAppointments { get; set; }
    public int TodayAppointments { get; set; }
    public List<AppointmentDto> UpcomingAppointments { get; set; } = new();
}
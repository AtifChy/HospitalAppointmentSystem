using BLL.DTOs;

namespace Web.Models;

public class AdminDashboardViewModel
{
    public int TotalDoctors { get; set; }
    public int TotalPatients { get; set; }
    public int TotalDepartments { get; set; }
    public int TotalAppointments { get; set; }
    public int PendingAppointments { get; set; }
    public int TodayAppointments { get; set; }
    public List<AppointmentDto> RecentAppointments { get; set; } = new();
}
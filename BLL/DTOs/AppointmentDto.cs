namespace BLL.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }

    public string DoctorName { get; set; }
    public string PatientName { get; set; }
    public string Department { get; set; }
}
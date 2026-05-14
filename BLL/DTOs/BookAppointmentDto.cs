using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class BookAppointmentDto
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string TimeSlot { get; set; }

    public string? Notes { get; set; }
}
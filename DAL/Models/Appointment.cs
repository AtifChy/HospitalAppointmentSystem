namespace DAL.Models;

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}

public class Appointment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan TimeSlot { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Foreign key
    public int DoctorId { get; set; }
    public int PatientId { get; set; }

    // Navigation
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }
    public Prescription? Prescription { get; set; }
}
namespace DAL.Models;

public class Patient
{
    public int Id { get; set; }
    public string BloodGroup { get; set; }
    public string EmergencyContact { get; set; }
    public string? MedicalHistory { get; set; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation
    public User User { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
}
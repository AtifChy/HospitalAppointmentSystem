namespace DAL.Models;

public class Prescription
{
    public int Id { get; set; }
    public string Medication { get; set; }
    public string Dosage { get; set; }
    public string Instruction { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.Now;

    // Foreign key
    public int AppointmentId { get; set; }
    public int DoctorId { get; set; }

    // Navigation
    public Appointment Appointment { get; set; }
    public Doctor Doctor { get; set; }
}
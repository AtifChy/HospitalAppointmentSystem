using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Specialty { get; set; }
    public string LicenseNumber { get; set; }

    [Precision(10, 2)] public decimal Fee { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Foreign key
    public int UserId { get; set; }
    public int DepartmentId { get; set; }

    // Navigation
    public User User { get; set; }
    public Department Department { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    public ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
}
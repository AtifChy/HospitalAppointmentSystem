namespace DAL.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Foreign key
    public int RoleId { get; set; }

    // Navigation
    public Role Role { get; set; }
    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
}
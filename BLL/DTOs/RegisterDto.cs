using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class RegisterDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    public string? BloodGroup { get; set; }
    public string? EmergencyContact { get; set; }
    public string? MedicalHistory { get; set; }
}
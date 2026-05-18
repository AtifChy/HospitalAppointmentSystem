using System.ComponentModel.DataAnnotations;
using BLL.Validation;

namespace BLL.DTOs;

public class RegisterDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    public string Name { get; set; }

    [Required]
    [CustomEmail]
    [UniqueEmail]
    public string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public string? Gender { get; set; }
    public string? Address { get; set; }

    [MaxLength(15, ErrorMessage = "Phone number must be 15 digits long")]
    public string PhoneNumber { get; set; }

    [MinAge(16)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string BloodGroup { get; set; }

    [Required]
    [MaxLength(15, ErrorMessage = "Phone number must be 15 digits long")]
    public string EmergencyContact { get; set; }

    public string? MedicalHistory { get; set; }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BLL.DTOs;

public class DoctorDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string LicenseNumber { get; set; }

    [Required]
    [Precision(10, 2)]
    public decimal Fee { get; set; }

    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }

    public bool IsAvailable { get; set; }
}
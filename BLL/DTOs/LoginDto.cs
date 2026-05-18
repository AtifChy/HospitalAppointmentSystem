using System.ComponentModel.DataAnnotations;
using BLL.Validation;

namespace BLL.DTOs;

public class LoginDto
{
    [Required]
    [CustomEmail]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
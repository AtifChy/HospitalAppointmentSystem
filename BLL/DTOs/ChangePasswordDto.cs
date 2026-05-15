using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmNewPassword { get; set; }
}
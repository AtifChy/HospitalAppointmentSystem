using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BLL.Validation;

public class CustomEmailAttribute : ValidationAttribute
{
    private readonly Regex EmailRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return new ValidationResult("Email cannot be null");

        var email = value.ToString();
        if (!EmailRegex.IsMatch(email)) return new ValidationResult("Invalid email format");

        return ValidationResult.Success;
    }
}
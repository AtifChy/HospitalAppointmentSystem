using System.ComponentModel.DataAnnotations;

namespace BLL.Validation;

public class MinAgeAttribute : ValidationAttribute
{
    private readonly int _minAge;

    public MinAgeAttribute(int minAge)
    {
        _minAge = minAge;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (age < _minAge) return new ValidationResult($"Age must be at least {_minAge}");

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid date of birth");
    }
}
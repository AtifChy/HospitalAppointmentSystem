using System.ComponentModel.DataAnnotations;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Validation;

public class UniqueLicenseAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("License cannot be null");

        var licenseNumber = value.ToString();

        var doctorService = validationContext.GetService<DoctorService>();
        if (doctorService == null)
            return new ValidationResult("Doctor service is not available");

        if (!doctorService.IsLicenseUnique(licenseNumber))
            return new ValidationResult("License is not unique");

        return ValidationResult.Success;
    }
}
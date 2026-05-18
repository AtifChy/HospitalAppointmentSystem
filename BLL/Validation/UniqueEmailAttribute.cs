using System.ComponentModel.DataAnnotations;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Validation;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("Email cannot be null");

        var email = value.ToString();
        var userRepository = validationContext.GetService<UserRepository>();
        if (userRepository == null)
            return new ValidationResult("User repository is not available");

        if (userRepository.GetByEmail(email) != null) return new ValidationResult("Email is already in use");

        return ValidationResult.Success;
    }
}
using BLL.DTOs;
using BLL.Services;
using DAL.Context;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Tests;

public class AuthServiceTests
{
    private AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsUserDto()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        var password = "password123";
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Name = "Test User",
            RoleId = 3
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var loginDto = new LoginDto { Email = "test@example.com", Password = password };

        // Act
        var result = await authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task LoginAsync_InvalidEmail_ReturnsNull()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        var loginDto = new LoginDto { Email = "nonexistent@example.com", Password = "password123" };

        // Act
        var result = await authService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            Name = "Test User",
            RoleId = 3
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var loginDto = new LoginDto { Email = "test@example.com", Password = "wrongpassword" };

        // Act
        var result = await authService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_ValidDto_ReturnsTrueAndCreatesUserAndPatient()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        var registerDto = new RegisterDto
        {
            Email = "newuser@example.com",
            Password = "password123",
            ConfirmPassword = "password123",
            Name = "New User",
            BloodGroup = "A+",
            EmergencyContact = "123456789",
            MedicalHistory = "None"
        };

        // Act
        var result = await authService.RegisterAsync(registerDto);

        // Assert
        Assert.True(result);
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
        Assert.NotNull(user);
        Assert.Equal(3, user.RoleId);

        var patient = await context.Patients.FirstOrDefaultAsync(p => p.UserId == user.Id);
        Assert.NotNull(patient);
        Assert.Equal("A+", patient.BloodGroup);
    }

    [Fact]
    public async Task RegisterAsync_ExistingEmail_ReturnsFalse()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        var existingUser = new User
        {
            Email = "existing@example.com",
            PasswordHash = "hash",
            Name = "Existing User",
            RoleId = 3
        };
        await context.Users.AddAsync(existingUser);
        await context.SaveChangesAsync();

        var registerDto = new RegisterDto
        {
            Email = "existing@example.com",
            Password = "password123",
            ConfirmPassword = "password123",
            Name = "New User"
        };

        // Act
        var result = await authService.RegisterAsync(registerDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ChangePassword_ValidCredentials_ReturnsTrueAndUpdatesPassword()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        var oldPassword = "oldpassword";
        var newPassword = "newpassword";
        var user = new User
        {
            Email = "user@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(oldPassword),
            Name = "Test User",
            RoleId = 3
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await authService.ChangePassword(user.Id, oldPassword, newPassword);

        // Assert
        Assert.True(result);
        var updatedUser = await context.Users.FindAsync(user.Id);
        Assert.True(BCrypt.Net.BCrypt.Verify(newPassword, updatedUser.PasswordHash));
    }

    [Fact]
    public async Task ChangePassword_InvalidOldPassword_ReturnsFalse()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        var user = new User
        {
            Email = "user@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctoldpassword"),
            Name = "Test User",
            RoleId = 3
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await authService.ChangePassword(user.Id, "wrongoldpassword", "newpassword");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ChangePassword_UserNotFound_ReturnsFalse()
    {
        // Arrange
        var context = GetDatabaseContext();
        var userRepository = new UserRepository(context);
        var patientRepository = new PatientRepository(context);
        var authService = new AuthService(patientRepository, userRepository);

        // Act
        var result = await authService.ChangePassword(999, "oldpassword", "newpassword");

        // Assert
        Assert.False(result);
    }
}
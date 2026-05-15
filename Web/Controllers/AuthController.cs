using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Login()
    {
        return View(new LoginDto());
    }

    public IActionResult Register()
    {
        return View(new RegisterDto());
    }

    public IActionResult ChangePassword()
    {
        return View();
    }
}
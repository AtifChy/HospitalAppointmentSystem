using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;

namespace Web.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (SessionHelper.IsLoggedIn(HttpContext.Session))
            return RedirectToAction("Index", "Home");

        return View(new LoginDto());
    }

    [HttpPost]
    public IActionResult Login(LoginDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var user = _authService.Login(dto);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(dto);
        }

        SessionHelper.SetUserSession(HttpContext.Session, user);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (SessionHelper.IsLoggedIn(HttpContext.Session))
            return RedirectToAction("Index", "Home");

        return View(new RegisterDto());
    }

    [HttpPost]
    public IActionResult Register(RegisterDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var success = _authService.Register(dto);
        if (!success)
        {
            ModelState.AddModelError("", "Email already exists");
            return View(dto);
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        if (SessionHelper.IsLoggedIn(HttpContext.Session))
            return RedirectToAction("Login");

        return View();
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return RedirectToAction("Login");

        var success = _authService.ChangePassword(userId.Value, dto.OldPassword, dto.NewPassword);
        if (!success)
        {
            ModelState.AddModelError("", "Old Password is incorrect");
            return View(dto);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        SessionHelper.ClearSession(HttpContext.Session);
        return RedirectToAction("Login");
    }
}
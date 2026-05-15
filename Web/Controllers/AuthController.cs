using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AuthController : Controller
{
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
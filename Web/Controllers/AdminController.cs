using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;

namespace Web.Controllers;

public class AdminController : Controller
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }

    private IActionResult? AdminOnly()
    {
        if (!SessionHelper.IsLoggedIn(HttpContext.Session))
            return RedirectToAction("Login", "Auth");
        if (!SessionHelper.IsAdmin(HttpContext.Session))
            return RedirectToAction("Index", "Home");
        return null;
    }

    public IActionResult Index()
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var admins = _adminService.GetAllAdmins();
        return View(admins);
    }

    public IActionResult Create()
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        return View(new AdminDto());
    }

    [HttpPost]
    public IActionResult Create(AdminDto dto)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var success = _adminService.AddAdmin(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Admin with this email already exists";
            return View(dto);
        }

        TempData["SuccessMessage"] = "Admin added successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var admin = _adminService.GetAdminById(id);
        if (admin == null) return NotFound();

        return View(admin);
    }

    [HttpPost]
    public IActionResult Edit(AdminDto dto)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var success = _adminService.UpdateAdmin(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to update admin";
            return View(dto);
        }

        TempData["SuccessMessage"] = "Admin updated successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var admin = _adminService.GetAdminById(id);
        if (admin == null) return NotFound();

        return View(admin);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        _adminService.Delete(id);
        TempData["SuccessMessage"] = "Admin deleted successfully";
        return RedirectToAction("Index");
    }
}

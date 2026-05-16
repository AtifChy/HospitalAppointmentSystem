using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;

namespace Web.Controllers;

public class DepartmentController : Controller
{
    private readonly DepartmentService _departmentService;

    public DepartmentController(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    private RedirectToActionResult? AdminOnly()
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

        var departments = _departmentService.GetAllDepartments();
        return View(departments);
    }

    public IActionResult Create(string? returnUrl = null)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        ViewBag.ReturnUrl = returnUrl;
        return View(new DepartmentDto());
    }

    [HttpPost]
    public IActionResult Create(DepartmentDto dto, string? returnUrl = null)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(dto);
        }

        var success = _departmentService.AddDepartment(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Department already exists";
            return View(dto);
        }

        TempData["SuccessMessage"] = "Department added successfully";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index");
    }


    public IActionResult Edit(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var department = _departmentService.GetDepartmentById(id);
        if (department == null) return NotFound();

        return View(department);
    }

    [HttpPost]
    public IActionResult Edit(DepartmentDto dto)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        if (!ModelState.IsValid) return View(dto);

        var success = _departmentService.UpdateDepartment(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Department updating failed";
            return View(dto);
        }

        TempData["SuccessMessage"] = "Department updated successfully";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var department = _departmentService.GetDepartmentById(id);
        if (department == null) return NotFound();

        return View(department);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        _departmentService.Delete(id);
        TempData["SuccessMessage"] = "Department deleted successfully";
        return RedirectToAction("Index");
    }
}
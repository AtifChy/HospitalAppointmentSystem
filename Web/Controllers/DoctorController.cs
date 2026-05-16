using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;

namespace Web.Controllers;

public class DoctorController : Controller
{
    private readonly DepartmentService _departmentService;
    private readonly DoctorService _doctorService;

    public DoctorController(DoctorService doctorService, DepartmentService departmentService)
    {
        _doctorService = doctorService;
        _departmentService = departmentService;
    }

    public IActionResult? AdminOnly()
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

        var doctors = _doctorService.GetAllDoctors();
        return View(doctors);
    }

    public IActionResult Create()
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        ViewBag.Departments = _departmentService.GetAll();
        return View(new DoctorDto());
    }

    [HttpPost]
    public IActionResult Create(DoctorDto dto)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        if (!ModelState.IsValid)
        {
            ViewBag.Departments = _departmentService.GetAll();
            return View(dto);
        }

        var success = _doctorService.AddDoctor(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Doctor already exists";
            ViewBag.Departments = _departmentService.GetAll();
            return View(dto);
        }

        TempData["SuccessMessage"] = "Doctor added successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var doctor = _doctorService.GetDoctorById(id);
        if (doctor == null) return NotFound();

        ViewBag.Departments = _departmentService.GetAll();
        return View(doctor);
    }

    [HttpPost]
    public IActionResult Edit(DoctorDto dto)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var success = _doctorService.UpdateDoctor(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Doctor already exists";
            ViewBag.Departments = _departmentService.GetAll();
            return View(dto);
        }

        TempData["SuccessMessage"] = "Doctor updated successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var doctor = _doctorService.GetDoctorById(id);
        if (doctor == null) return NotFound();

        return View(doctor);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        _doctorService.Delete(id);
        TempData["SuccessMessage"] = "Doctor deleted successfully";
        return RedirectToAction("Index");
    }
}
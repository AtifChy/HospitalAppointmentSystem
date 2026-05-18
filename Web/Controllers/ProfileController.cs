using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;

namespace Web.Controllers;

public class ProfileController : Controller
{
    private readonly AdminService _adminService;
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;

    public ProfileController(AdminService adminService, DoctorService doctorService, PatientService patientService)
    {
        _adminService = adminService;
        _doctorService = doctorService;
        _patientService = patientService;
    }

    public IActionResult Index()
    {
        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return RedirectToAction("Login", "Auth");

        var role = SessionHelper.GetUserRole(HttpContext.Session);

        if (role == "Admin")
        {
            var admin = _adminService.GetAdminById(userId.Value);
            return View("AdminProfile", admin);
        }

        if (role == "Doctor")
        {
            var doctor = _doctorService.GetDoctorByUserId(userId.Value);
            return View("DoctorProfile", doctor);
        }

        if (role == "Patient")
        {
            var patient = _patientService.GetPatientByUserId(userId.Value);
            return View("PatientProfile", patient);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Edit()
    {
        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return RedirectToAction("Login", "Auth");

        var role = SessionHelper.GetUserRole(HttpContext.Session);

        if (role == "Admin")
        {
            var admin = _adminService.GetAdminById(userId.Value);
            return View("EditAdmin", admin);
        }

        if (role == "Doctor")
        {
            var doctor = _doctorService.GetDoctorByUserId(userId.Value);
            return View("EditDoctor", doctor);
        }

        if (role == "Patient")
        {
            var patient = _patientService.GetPatientByUserId(userId.Value);
            return View("EditPatient", patient);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult EditAdmin(AdminDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var success = _adminService.UpdateAdmin(dto);
        if (success)
        {
            TempData["SuccessMessage"] = "Profile updated successfully";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Failed to update profile";
        return View(dto);
    }

    [HttpPost]
    public IActionResult EditDoctor(DoctorDto dto)
    {
        // Remove password validation if not provided for update
        ModelState.Remove("Password");

        if (!ModelState.IsValid) return View(dto);

        var success = _doctorService.UpdateDoctor(dto);
        if (success)
        {
            TempData["SuccessMessage"] = "Profile updated successfully";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Failed to update profile";
        return View(dto);
    }

    [HttpPost]
    public IActionResult EditPatient(PatientDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var success = _patientService.UpdatePatient(dto);
        if (success)
        {
            TempData["SuccessMessage"] = "Profile updated successfully";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Failed to update profile";
        return View(dto);
    }
}
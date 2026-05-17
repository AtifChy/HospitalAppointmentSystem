using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;

namespace Web.Controllers;

public class PatientController : Controller
{
    private readonly PatientService _patientService;

    public PatientController(PatientService patientService)
    {
        _patientService = patientService;
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

        var patients = _patientService.GetAllPatients();
        return View(patients);
    }

    public IActionResult Create()
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        return View(new PatientDto());
    }

    [HttpPost]
    public IActionResult Create(PatientDto dto)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var success = _patientService.AddPatient(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Patient already exists";
            return View(dto);
        }

        TempData["SuccessMessage"] = "Patient added successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var patient = _patientService.GetPatientById(id);
        if (patient == null) return NotFound();

        return View(patient);
    }

    public IActionResult Edit(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var patient = _patientService.GetPatientById(id);
        if (patient == null) return NotFound();

        return View(patient);
    }

    [HttpPost]
    public IActionResult Edit(PatientDto dto)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var success = _patientService.UpdatePatient(dto);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to update patient";
            return View(dto);
        }

        TempData["SuccessMessage"] = "Patient updated successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var patient = _patientService.GetPatientById(id);
        if (patient == null) return NotFound();

        return View(patient);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        _patientService.Delete(id);
        TempData["SuccessMessage"] = "Patient deleted successfully";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult MarkForPasswordChange(int id)
    {
        var adminOnly = AdminOnly();
        if (adminOnly != null) return adminOnly;

        var success = _patientService.MarkForPasswordChange(id);
        if (success)
            TempData["SuccessMessage"] = "Patient marked for password change";
        else
            TempData["ErrorMessage"] = "Failed to mark patient for password change";

        return RedirectToAction("Index");
    }
}

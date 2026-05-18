using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;

namespace Web.Controllers;

public class AppointmentController : Controller
{
    private readonly AppointmentService _appointmentService;
    private readonly DepartmentService _departmentService;
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;

    public AppointmentController(
        AppointmentService appointmentService,
        DoctorService doctorService,
        PatientService patientService,
        DepartmentService departmentService
    )
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _patientService = patientService;
        _departmentService = departmentService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Book()
    {
        if (!SessionHelper.IsPatient(HttpContext.Session))
            return RedirectToAction("Login", "Auth");

        ViewBag.Departments = _departmentService.GetAllDepartments();
        ViewBag.Doctors = _doctorService.GetAllDoctors();
        return View(new BookAppointmentDto());
    }

    [HttpPost]
    public IActionResult Book(BookAppointmentDto dto)
    {
        if (!SessionHelper.IsPatient(HttpContext.Session))
            return RedirectToAction("Login", "Auth");

        /*if (!ModelState.IsValid)
        {
            ViewBag.Departments = _departmentService.GetAllDepartments();
            ViewBag.Doctors = _doctorService.GetAllDoctors();
            return View(dto);
        }*/

        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return RedirectToAction("Login", "Auth");

        var patient = _patientService.GetPatientByUserId(userId.Value);
        if (patient == null) return RedirectToAction("Login", "Auth");

        var success = _appointmentService.BookAppointment(dto, patient.Id);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to book appointment";
            ViewBag.Departments = _departmentService.GetAllDepartments();
            ViewBag.Doctors = _doctorService.GetAllDoctors();
            return View(dto);
        }

        TempData["SuccessMessage"] = "Appointment booked successfully";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AddPrescription(int id)
    {
        if (!SessionHelper.IsLoggedIn(HttpContext.Session) && !SessionHelper.IsDoctor(HttpContext.Session))
            return RedirectToAction("Login", "Auth");

        var appointment = _appointmentService.GetAppointmentWithDetails(id);
        if (appointment == null) return RedirectToAction("Index", "Home");

        if (appointment.Status != "Confirmed" && appointment.Status != "Completed")
        {
            TempData["ErrorMessage"] = "You can only add prescription for confirmed or completed appointments";
            return RedirectToAction("Index", "Home");
        }

        ViewBag.PatientName = appointment.PatientName;

        var dto = new PrescriptionDto
        {
            AppointmentId = id,
            Medication = appointment.Prescription?.Medication ?? "",
            Dosage = appointment.Prescription?.Dosage ?? "",
            Instruction = appointment.Prescription?.Instruction ?? ""
        };

        return View(dto);
    }

    [HttpPost]
    public IActionResult AddPrescription(PrescriptionDto dto)
    {
        if (!SessionHelper.IsLoggedIn(HttpContext.Session) && !SessionHelper.IsDoctor(HttpContext.Session))
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid) return View(dto);

        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return RedirectToAction("Login", "Auth");

        var doctor = _doctorService.GetDoctorByUserId(userId.Value);
        if (doctor == null) return RedirectToAction("Login", "Auth");

        var success = _appointmentService.SavePrescription(dto, doctor.Id);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to save prescription";
            return View(dto);
        }

        TempData["SuccessMessage"] = "Prescription saved successfully";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public JsonResult GetDoctorsByDepartment(int departmentId)
    {
        var doctors = _doctorService.GetDoctorsByDepartmentId(departmentId);
        return Json(doctors.Select(d => new
        {
            id = d.Id,
            name = d.Name,
            fee = d.Fee
        }));
    }
}
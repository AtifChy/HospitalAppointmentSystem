using System.Diagnostics;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly AppointmentService _appointmentService;
    private readonly DepartmentService _departmentService;
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;

    public HomeController(AppointmentService appointmentService, DepartmentService departmentService,
        DoctorService doctorService, PatientService patientService)
    {
        _appointmentService = appointmentService;
        _departmentService = departmentService;
        _doctorService = doctorService;
        _patientService = patientService;
    }

    public IActionResult Index()
    {
        if (!SessionHelper.IsLoggedIn(HttpContext.Session))
            return RedirectToAction("Login", "Auth");

        if (SessionHelper.MustChangePassword(HttpContext.Session))
            return RedirectToAction("ChangePassword", "Auth");

        var role = SessionHelper.GetUserRole(HttpContext.Session);

        switch (role)
        {
            case "Admin":
                return View("AdminDashboard", GetAdminDashboardViewModel());
            case "Doctor":
                return View("DoctorDashboard", GetDoctorDashboardViewModel());
            case "Patient":
                return View("PatientDashboard", GetPatientDashboardViewModel());
            default:
                return RedirectToAction("Login", "Auth");
        }
    }

    private AdminDashboardViewModel GetAdminDashboardViewModel()
    {
        var doctors = _doctorService.GetAllDoctors();
        var departments = _departmentService.GetAll();
        var patients = _patientService.GetAll();
        var appointments = _appointmentService.GetAllAppointments();

        return new AdminDashboardViewModel
        {
            TotalDoctors = doctors.Count,
            TotalDepartments = departments.Count,
            TotalPatients = patients.Count,
            TotalAppointments = appointments.Count,
            PendingAppointments = appointments.Count(a => a.Status == "Pending"),
            TodayAppointments = appointments.Count(a => a.Date.Date == DateTime.Today),
            RecentAppointments = _appointmentService.GetRecentAppointments(5)
        };
    }

    private DoctorDashboardViewModel GetDoctorDashboardViewModel()
    {
        if (!SessionHelper.IsDoctor(HttpContext.Session)) return new DoctorDashboardViewModel();

        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return new DoctorDashboardViewModel();

        var doctor = _doctorService.GetDoctorByUserId(userId.Value);
        if (doctor == null) return new DoctorDashboardViewModel();

        var appointments = _appointmentService.GetByDoctorId(doctor.Id);

        return new DoctorDashboardViewModel
        {
            TotalAppointments = appointments.Count,
            PendingAppointments = appointments.Count(a => a.Status == "Pending"),
            TodayAppointments = appointments.Count(a => a.Date.Date == DateTime.Today),
            ConfirmedAppointments = appointments.Count(a => a.Status == "Confirmed"),
            UpcomingAppointments = appointments
                .Where(a => a.Date.Date >= DateTime.Today && a.Status != "Cancelled")
                .OrderBy(a => a.Date)
                .Take(5)
                .ToList()
        };
    }

    private PatientDashboardViewModel GetPatientDashboardViewModel()
    {
        if (!SessionHelper.IsPatient(HttpContext.Session)) return new PatientDashboardViewModel();

        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return new PatientDashboardViewModel();

        var patient = _patientService.GetPatientByUserId(userId.Value);
        if (patient == null) return new PatientDashboardViewModel();

        var appointments = _appointmentService.GetByPatientId(patient.Id);

        return new PatientDashboardViewModel
        {
            TotalAppointments = appointments.Count,
            PendingAppointments = appointments.Count(a => a.Status == "Pending"),
            CompletedAppointments = appointments.Count(a => a.Status == "Completed"),
            UpcomingAppointments = appointments
                .Where(a => a.Date.Date >= DateTime.Today && a.Status != "Cancelled")
                .OrderBy(a => a.Date)
                .Take(5)
                .ToList()
        };
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult ConfirmAppointment(int id)
    {
        if (!SessionHelper.IsDoctor(HttpContext.Session)) return RedirectToAction("Index", "Home");

        var success = _appointmentService.UpdateStatus(id, "Confirmed");
        if (success) TempData["SuccessMessage"] = "Appointment confirmed";
        else TempData["ErrorMessage"] = "Failed to confirm appointment";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CancelAppointment(int id)
    {
        if (!SessionHelper.IsDoctor(HttpContext.Session)) return RedirectToAction("Index", "Home");

        var success = _appointmentService.UpdateStatus(id, "Cancelled");
        if (success) TempData["SuccessMessage"] = "Appointment cancelled";
        else TempData["ErrorMessage"] = "Failed to cancel appointment";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CompleteAppointment(int id)
    {
        if (!SessionHelper.IsDoctor(HttpContext.Session)) return RedirectToAction("Index", "Home");

        var success = _appointmentService.UpdateStatus(id, "Completed");
        if (success) TempData["SuccessMessage"] = "Appointment completed";
        else TempData["ErrorMessage"] = "Failed to complete appointment. Please add prescription first.";

        return RedirectToAction("Index");
    }
}
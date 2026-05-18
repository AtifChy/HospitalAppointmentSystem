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

    public IActionResult Index(string sortOrder)
    {
        if (!SessionHelper.IsLoggedIn(HttpContext.Session))
            return RedirectToAction("Login", "Auth");

        ViewBag.DateSort = sortOrder == "date_desc" ? "date_asc" : "date_desc";
        ViewBag.TimeSort = sortOrder == "time_desc" ? "time_asc" : "time_desc";
        ViewBag.DoctorSort = sortOrder == "doctor_desc" ? "doctor_asc" : "doctor_desc";
        ViewBag.PatientSort = sortOrder == "patient_desc" ? "patient_asc" : "patient_desc";
        ViewBag.DepartmentSort = sortOrder == "department_desc" ? "department_asc" : "department_desc";
        ViewBag.StatusSort = sortOrder == "status_desc" ? "status_asc" : "status_desc";

        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return RedirectToAction("Login", "Auth");

        List<AppointmentDto> appointments;

        if (SessionHelper.IsAdmin(HttpContext.Session))
        {
            appointments = _appointmentService.GetAllAppointments();
        }
        else if (SessionHelper.IsDoctor(HttpContext.Session))
        {
            var doctor = _doctorService.GetDoctorByUserId(userId.Value);
            if (doctor == null) return RedirectToAction("Login", "Auth");
            appointments = _appointmentService.GetByDoctorId(doctor.Id);
        }
        else if (SessionHelper.IsPatient(HttpContext.Session))
        {
            var patient = _patientService.GetPatientByUserId(userId.Value);
            if (patient == null) return RedirectToAction("Login", "Auth");
            appointments = _appointmentService.GetByPatientId(patient.Id);
        }
        else
        {
            return RedirectToAction("Login", "Auth");
        }

        appointments = sortOrder switch
        {
            "date_desc" => appointments.OrderByDescending(a => a.Date).ToList(),
            "time_asc" => appointments.OrderBy(a => a.TimeSlot).ToList(),
            "time_desc" => appointments.OrderByDescending(a => a.TimeSlot).ToList(),
            "doctor_asc" => appointments.OrderBy(a => a.DoctorName).ToList(),
            "doctor_desc" => appointments.OrderByDescending(a => a.DoctorName).ToList(),
            "dept_asc" => appointments.OrderBy(a => a.Department).ToList(),
            "dept_desc" => appointments.OrderByDescending(a => a.Department).ToList(),
            "patient_asc" => appointments.OrderBy(a => a.PatientName).ToList(),
            "patient_desc" => appointments.OrderByDescending(a => a.PatientName).ToList(),
            "status_asc" => appointments.OrderBy(a => a.Status).ToList(),
            "status_desc" => appointments.OrderByDescending(a => a.Status).ToList(),
            _ => appointments.OrderBy(a => a.Date).ToList()
        };

        return View(appointments);
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

        ViewBag.Departments = _departmentService.GetAllDepartments();
        ViewBag.Doctors = _doctorService.GetAllDoctors();

        if (dto.Date < DateTime.Today)
        {
            TempData["ErrorMessage"] = "Appointment date cannot be in the past";
            return View(dto);
        }

        var slotTaken = _doctorService.IsSlotTaken(dto.DoctorId, dto.Date, TimeSpan.Parse(dto.TimeSlot));
        if (slotTaken)
        {
            TempData["ErrorMessage"] = "Slot is already taken";
            return View(dto);
        }

        var userId = SessionHelper.GetUserId(HttpContext.Session);
        if (userId == null) return RedirectToAction("Login", "Auth");

        var patient = _patientService.GetPatientByUserId(userId.Value);
        if (patient == null) return RedirectToAction("Login", "Auth");

        var success = _appointmentService.BookAppointment(dto, patient.Id);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to book appointment";
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

    [HttpGet]
    public IActionResult ViewPrescription(int id)
    {
        if (!SessionHelper.IsLoggedIn(HttpContext.Session))
            return RedirectToAction("Login", "Auth");

        var appointment = _appointmentService.GetAppointmentWithDetails(id);
        if (appointment == null)
        {
            TempData["ErrorMessage"] = "Appointment not found";
            return RedirectToAction("Index", "Home");
        }

        return View(appointment);
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
    public JsonResult GetActiveDoctorsByDepartment(int departmentId)
    {
        var doctors = _doctorService.GetActiveDoctorsByDepartmentId(departmentId);
        return Json(doctors.Select(d => new
        {
            id = d.Id,
            name = d.Name,
            fee = d.Fee
        }));
    }
}
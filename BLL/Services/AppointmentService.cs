using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class AppointmentService : GenericService<Appointment>
{
    private readonly AppointmentRepository _appointmentRepository;
    private readonly Mapper _mapper;
    private readonly PrescriptionRepository _prescriptionRepository;

    public AppointmentService(AppointmentRepository appointmentRepository,
        PrescriptionRepository prescriptionRepository) : base(appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
        _prescriptionRepository = prescriptionRepository;
        _mapper = MapperConfig.GetMapper();
    }

    public List<AppointmentDto> GetAllAppointments()
    {
        var appointments = _appointmentRepository.GetAll();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public List<AppointmentDto> GetRecentAppointments(int count)
    {
        var appointments = _appointmentRepository.GetRecentAppointments(count);
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public AppointmentDto? GetAppointmentWithDetails(int id)
    {
        var appointment = _appointmentRepository.GetAppointmentWithDetails(id);
        if (appointment == null) return null;
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public List<AppointmentDto> GetByDoctorId(int doctorId)
    {
        var appointments = _appointmentRepository.GetByDoctorId(doctorId);
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public List<AppointmentDto> GetByPatientId(int patientId)
    {
        var appointments = _appointmentRepository.GetByPatientId(patientId);
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public bool BookAppointment(BookAppointmentDto dto, int patientId)
    {
        var appointment = new Appointment
        {
            DoctorId = dto.DoctorId,
            PatientId = patientId,
            Date = dto.Date,
            TimeSlot = TimeSpan.Parse(dto.TimeSlot),
            Notes = dto.Notes,
            Status = AppointmentStatus.Pending
        };

        _appointmentRepository.Add(appointment);
        return true;
    }

    public bool SavePrescription(PrescriptionDto dto, int doctorId)
    {
        var appointment = _appointmentRepository.GetAppointmentWithDetails(dto.AppointmentId);
        if (appointment == null) return false;

        if (appointment.Status != AppointmentStatus.Confirmed &&
            appointment.Status != AppointmentStatus.Completed) return false;

        if (appointment.Prescription == null)
        {
            // create prescription
            var prescription = new Prescription
            {
                AppointmentId = dto.AppointmentId,
                DoctorId = doctorId,
                Medication = dto.Medication,
                Dosage = dto.Dosage,
                Instruction = dto.Instruction
            };
            _prescriptionRepository.Add(prescription);
        }
        else
        {
            // update prescription
            appointment.Prescription.Medication = dto.Medication;
            appointment.Prescription.Dosage = dto.Dosage;
            appointment.Prescription.Instruction = dto.Instruction;
            _appointmentRepository.Update(appointment);
        }

        return true;
    }

    public bool UpdateStatus(int appointmentId, string status)
    {
        var appointment = _appointmentRepository.GetById(appointmentId);
        if (appointment == null) return false;

        if (Enum.TryParse<AppointmentStatus>(status, true, out var appointmentStatus))
        {
            if (appointmentStatus == AppointmentStatus.Completed && appointment.Prescription == null)
                return false;

            appointment.Status = appointmentStatus;
            _appointmentRepository.Update(appointment);
            return true;
        }

        return false;
    }
}
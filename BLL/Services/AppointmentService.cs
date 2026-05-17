using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class AppointmentService : GenericService<Appointment>
{
    private readonly AppointmentRepository _appointmentRepository;
    private readonly Mapper _mapper;

    public AppointmentService(AppointmentRepository appointmentRepository) : base(appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
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
}
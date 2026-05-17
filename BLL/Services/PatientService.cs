using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class PatientService : GenericService<Patient>
{
    private readonly Mapper _mapper;
    private readonly PatientRepository _patientRepository;
    private readonly UserRepository _userRepository;

    public PatientService(PatientRepository patientRepository, UserRepository userRepository) : base(patientRepository)
    {
        _patientRepository = patientRepository;
        _userRepository = userRepository;
        _mapper = MapperConfig.GetMapper();
    }

    public List<PatientDto> GetAllPatients()
    {
        var patients = _patientRepository.GetPatientsAll();
        return _mapper.Map<List<PatientDto>>(patients);
    }

    public PatientDto? GetPatientById(int id)
    {
        var patient = _patientRepository.GetPatientById(id);
        if (patient == null) return null;
        return _mapper.Map<PatientDto>(patient);
    }

    public PatientDto? GetPatientByUserId(int id)
    {
        var patient = _patientRepository.GetPatientByUserId(id);
        if (patient == null) return null;
        return _mapper.Map<PatientDto>(patient);
    }

    public bool AddPatient(PatientDto dto)
    {
        var existing = _userRepository.GetByEmail(dto.Email);
        if (existing != null) return false;

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password ?? "Patient123!");
        user.RoleId = 3; // Role 3 is Patient
        user.MustChangePassword = true;
        _userRepository.Add(user);

        var patient = _mapper.Map<Patient>(dto);
        patient.UserId = user.Id;
        _patientRepository.Add(patient);

        return true;
    }

    public bool UpdatePatient(PatientDto dto)
    {
        var patient = _patientRepository.GetPatientById(dto.Id);
        if (patient == null) return false;

        // update user properties
        _mapper.Map(dto, patient.User);
        // update patient properties
        _mapper.Map(dto, patient);

        _userRepository.Update(patient.User);
        _patientRepository.Update(patient);

        return true;
    }

    public bool DeletePatient(int id)
    {
        _patientRepository.Delete(id);
        return true;
    }

    public bool MarkForPasswordChange(int id)
    {
        var patient = _patientRepository.GetPatientById(id);
        if (patient == null) return false;

        patient.User.MustChangePassword = true;
        _userRepository.Update(patient.User);

        return true;
    }
}
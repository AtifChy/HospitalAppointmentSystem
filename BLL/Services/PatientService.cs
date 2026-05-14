using DAL.Repositories;

namespace BLL.Services;

public class PatientService
{
    private readonly PatientRepository _patientRepository;

    public PatientService(PatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
}
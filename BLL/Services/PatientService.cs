using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class PatientService : GenericService<Patient>
{
    private readonly PatientRepository _patientRepository;

    public PatientService(PatientRepository patientRepository) : base(patientRepository)
    {
        _patientRepository = patientRepository;
    }
}
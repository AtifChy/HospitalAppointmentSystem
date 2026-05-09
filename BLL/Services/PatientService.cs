using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class PatientService : GenericService<Patient>
{
    public PatientService(PatientRepository repository) : base(repository)
    {
    }
}
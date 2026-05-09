using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class DoctorService : GenericService<Doctor>
{
    public DoctorService(DoctorRepository repository) : base(repository)
    {
    }
}
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services;

public class PrescriptionService : GenericService<Prescription>
{
    public PrescriptionService(PrescriptionRepository repository) : base(repository)
    {
    }
}
using DAL.Repositories;

namespace BLL.Services;

public class PrescriptionService
{
    private readonly PrescriptionRepository _prescriptionRepository;

    public PrescriptionService(PrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }
}
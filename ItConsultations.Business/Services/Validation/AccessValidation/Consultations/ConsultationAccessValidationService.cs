using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.AccessValidation.Consultations;

public class ConsultationAccessValidationService : AccessValidationServiceBase, IConsultationAccessValidationService
{
    private readonly IConsultationService _consultationService;

    public ConsultationAccessValidationService(IConsultationService consultationService)
    {
        _consultationService = consultationService;
    }

    public void ValidateConsultationAccessAsync(long id, string consId)
    {
        var consultation = _consultationService.Get(consId);

        if (consultation == null)
        {
            throw new ArgumentException($"Consultation with consId {consId} not found");
        }


    }
}

using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Utilities.Guards;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.Access.Consultations;

public class ConsultationAccessValidationService : AccessValidationServiceBase, IConsultationAccessValidationService
{
    private readonly IConsultationService _consultationService;

    public ConsultationAccessValidationService(
        IConsultationService consultationService
        )
    {
        _consultationService = consultationService;
    }

    public void ValidateConsultationAccessAsync(long id, string consId)
    {
        var consultation = _consultationService.GetAsync(consId).Result;

        if (consultation == null)
        {
            throw new ArgumentException($"Consultation with consId {consId} not found");
        }

        Guard.That(consultation.Coach?.CoachConsId != consId, "User doesn't have access to consultation.");
        Guard.That(consultation.Coach == null, "Consultation does not have a coach for targeted consultation.");
    }
}

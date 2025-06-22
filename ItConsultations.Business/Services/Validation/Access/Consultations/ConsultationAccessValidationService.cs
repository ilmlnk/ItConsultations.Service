using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Utilities.Guards;

namespace ItConsultations.Business.Services.Validation.Access.Consultations;

public class ConsultationAccessValidationService : IConsultationAccessValidationService
{
    private readonly IConsultationService _consultationService;

    public ConsultationAccessValidationService(
        IConsultationService consultationService
        )
    {
        _consultationService = consultationService;
    }

    public async void ValidateConsultationAccessAsync(long id, string consId)
    {
        var consultation = await _consultationService.GetAsync(consId);

        Guard.NotNull(consultation);
        Guard.That(consultation.Coach?.CoachConsId != consId, "User doesn't have access to consultation.");
        Guard.That(consultation.Coach == null, "Consultation does not have a coach for targeted consultation.");
    }
}

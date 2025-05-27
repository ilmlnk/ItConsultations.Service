using ItConsultations.Business.Services.ConsultationService;

namespace ItConsultations.Business.Services.Validation;

public class ConsultationAccessValidationService : IConsultationAccessValidationService
{
    private readonly IConsultationService _consultationService;

    public ConsultationAccessValidationService(
        IConsultationService consultationService
        ) 
    {
        _consultationService = consultationService;
    }

    public void ValidateUpdateConsultationAccess(long id, string consId)
    {
        
    }
}

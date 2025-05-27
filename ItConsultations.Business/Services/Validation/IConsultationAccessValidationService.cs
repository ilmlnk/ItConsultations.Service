namespace ItConsultations.Business.Services.Validation;

public interface IConsultationAccessValidationService
{
    void ValidateUpdateConsultationAccess(long id, string consId);
}

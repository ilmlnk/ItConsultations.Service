namespace ItConsultations.Business.Services.Validation.Access.Consultations;

public interface IConsultationAccessValidationService
{
    void ValidateUpdateConsultationAccess(long id, string consId);
}

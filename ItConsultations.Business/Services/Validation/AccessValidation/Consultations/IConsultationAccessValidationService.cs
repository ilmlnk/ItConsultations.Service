namespace ItConsultations.Business.Services.Validation.AccessValidation.Consultations;

public interface IConsultationAccessValidationService
{
    void ValidateConsultationAccessAsync(long id, string consId);
}

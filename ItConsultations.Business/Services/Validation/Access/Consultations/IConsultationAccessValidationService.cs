namespace ItConsultations.Business.Services.Validation.Access.Consultations;

public interface IConsultationAccessValidationService
{
    void ValidateConsultationAccessAsync(long id, string consId);
}

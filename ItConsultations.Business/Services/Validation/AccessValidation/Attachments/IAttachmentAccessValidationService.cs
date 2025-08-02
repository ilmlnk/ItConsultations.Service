namespace ItConsultations.Business.Services.Validation.AccessValidation.Attachments;

public interface IAttachmentAccessValidationService
{
    void ValidateAttachmentAccessAsync(long id, string consId);
}

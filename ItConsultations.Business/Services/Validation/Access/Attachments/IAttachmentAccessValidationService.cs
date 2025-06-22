namespace ItConsultations.Business.Services.Validation.Access.Attachments;

public interface IAttachmentAccessValidationService
{
    void ValidateAttachmentAccessAsync(long id, string consId);
}

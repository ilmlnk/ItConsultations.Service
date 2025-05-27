namespace ItConsultations.Business.Services.Validation.Access.Attachments;

public interface IAttachmentAccessValidationService
{
    void ValidateAccessToModifyAttachments();

    void ValidateAccessToGetAttachments();

    void ValidateAccessToAddAttachments();
}

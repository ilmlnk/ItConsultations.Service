using ItConsultations.Business.Services.AttachmentService;

namespace ItConsultations.Business.Services.Validation.Access.Attachments;

public class AttachmentAccessValidationService : IAttachmentAccessValidationService
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentAccessValidationService(
        IAttachmentService attachmentService
        )
    {
        _attachmentService = attachmentService;
    }

    public async void ValidateAttachmentAccessAsync(long id, string consId)
    {
        var attachment = await _attachmentService.GetAsync(consId);
    }
}

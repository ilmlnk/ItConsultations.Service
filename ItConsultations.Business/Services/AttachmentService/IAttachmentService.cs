using ItConsultations.Business.Entities.Attachments;

namespace ItConsultations.Business.Services.AttachmentService;

public interface IAttachmentService
{
    Task DeleteAsync(long id);

    Task<Attachment> GetAsync(string consId);
}

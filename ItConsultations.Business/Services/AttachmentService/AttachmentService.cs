using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Utilities.Guards;

namespace ItConsultations.Business.Services.AttachmentService;

public class AttachmentService : IAttachmentService
{
    private readonly IRepository<Attachment, long> _repository;

    public AttachmentService(IRepository<Attachment, long> repository)
    {
        _repository = repository;
    }

    public async Task DeleteAsync(long id)
    {
        var attachment = await _repository.GetAsync(id);
        Guard.NotNull(attachment);
        await _repository.DeleteAsync(attachment);
    }

    public async Task<Attachment> GetAsync(long id)
    {
        var attachment = await _repository.GetAsync(id);
        Guard.NotNull(attachment);
        return attachment;
    }

    public Task<Attachment> GetAsync(string consId)
    {
        throw new NotImplementedException();
    }
}

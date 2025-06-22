using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Entities.Attachments;

namespace ItConsultations.Business.Services.AttachmentService;

public class AttachmentService : IAttachmentService
{
    private readonly IRepository<Attachment, long> _repository;

    public AttachmentService(IRepository<Attachment, long> repository)
    {
        _repository = repository;
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<Attachment> GetAsync(string consId)
    {
        throw new NotImplementedException();
    }
}

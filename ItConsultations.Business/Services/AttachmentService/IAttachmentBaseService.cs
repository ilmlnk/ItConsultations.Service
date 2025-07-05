using ItConsultations.Business.Dtos.AttachmentDtos;
using ItConsultations.Business.Dtos.FileDtos;

namespace ItConsultations.Business.Services.AttachmentService;

public interface IAttachmentBaseService
{
    Task<AttachmentDto> UploadAsync(FileDto dto);

    Task<AttachmentDto> UpdateAsync(AttachmentDto dto);

    Task<AttachmentDto> DeleteAsync(long id);

    Task<AttachmentDto> GetAsync(long id);
}
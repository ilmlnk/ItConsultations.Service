using ItConsultations.Business.Dtos.AttachmentDtos;

namespace ItConsultations.Business.Services.AttachmentService.NormalizeService;

public interface IAttachmentNormalizationService
{
    Task<AttachmentDto> NormalizeAsync(AttachmentDto dto);
}

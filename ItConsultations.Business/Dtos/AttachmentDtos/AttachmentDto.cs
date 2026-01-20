using ItConsultations.Business.Dtos.FileDtos;

namespace ItConsultations.Business.Dtos.AttachmentDtos;

public class AttachmentDto
{
    public long Id { get; set; }

    public FileDto? File { get; set; }
}

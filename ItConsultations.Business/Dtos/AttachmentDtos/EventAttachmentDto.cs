using ItConsultations.Business.Entities.Attachments;

namespace ItConsultations.Business.Dtos.EventDtos;

public class EventAttachmentDto
{
    public long Id { get; set; }

    public string AttachmentConsId { get; set; } = string.Empty;

    public string EventId { get; set; }

    public long AttachmentId { get; set; }

    public Attachment Attachment { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsRequired { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
} 
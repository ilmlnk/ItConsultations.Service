using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Attachments;

namespace ItConsultations.Business.Entities.Events;

public class EventAttachment : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string AttachmentConsId { get; set; } = string.Empty;

    [Required]
    public string EventId { get; set; }

    [Required]
    public Event Event { get; set; } = null!;

    [Required]
    public long AttachmentId { get; set; }

    [Required]
    public Attachment Attachment { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsRequired { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 
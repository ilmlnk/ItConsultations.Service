using ItConsultations.Business.Entities.Attachments;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.Consultation;

public class Review
{
    [Required]
    [StringLength(32)]
    public long Id { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int Rating { get; set; }

    public Entity<long> Reviewer { get; set; }

    public List<Attachment> Attachments { get; set; }
}

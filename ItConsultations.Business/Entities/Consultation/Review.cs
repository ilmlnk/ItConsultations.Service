using ItConsultations.Business.Entities.Attachments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Consultation;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(32)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string ReviewConsId { get; set; }

    [Required]
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Required]
    public int Rating { get; set; }

    [Required]
    public User.UserEntity Reviewer { get; set; }

    public List<Attachment> Attachments { get; set; }
}

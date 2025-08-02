using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Reviews;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

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
    public UserEntity Reviewer { get; set; }

    public List<Attachment> Attachments { get; set; }
}

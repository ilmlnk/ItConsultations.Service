using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Article;

public class Article : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(32)]
    [Required]
    public long Id { get; set; }

    [MaxLength(32)]
    public string ArticleConsId { get; set; }

    [MaxLength(500)]
    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User.User CreatedBy { get; set; }

    public List<Attachment> Attachments { get; set; }
}

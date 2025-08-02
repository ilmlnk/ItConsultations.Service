using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ItConsultations.Business.Entities.Articles;

public class Article : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string ArticleConsId { get; set; }

    [MaxLength(500)]
    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public UserEntity CreatedBy { get; set; }

    [JsonIgnore]
    public List<Attachment> Attachments { get; set; } = new();
}

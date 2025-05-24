using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.Article;

public class Article : Entity<long>
{
    [Required]
    [MaxLength(32)]
    public Guid Id { get; set; }

    [MaxLength(32)]
    public string ArticleConsId { get; set; }

    [MaxLength(500)]
    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Entity<long> CreatedBy { get; set; }
}

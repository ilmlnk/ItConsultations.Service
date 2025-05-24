using ItConsultations.Business.Entities;

namespace ItConsultations.Business.Dtos.ArticleDtos;

public class ArticleDto
{
    public long Id { get; set; }

    public string ArticleConsId { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Entity<long> CreatedBy { get; set; }
}

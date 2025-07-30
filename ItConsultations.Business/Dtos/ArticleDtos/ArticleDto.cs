using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.Users;

namespace ItConsultations.Business.Dtos.ArticleDtos;

public class ArticleDto
{
    public long Id { get; set; }

    public string ArticleConsId { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserEntity CreatedBy { get; set; }

    public string? CoachConsId { get; set; }

    public string? StudentConsId { get; set; }

    public List<Attachment> Attachments { get; set; }
}

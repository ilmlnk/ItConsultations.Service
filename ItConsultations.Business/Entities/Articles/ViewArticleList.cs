using ItConsultations.Business.Entities.Users;

namespace ItConsultations.Business.Entities.Articles;

public class ViewArticleList
{
    public long Id { get; set; }

    public string ArticleConsId { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserEntity CreatedBy { get; set; }
}

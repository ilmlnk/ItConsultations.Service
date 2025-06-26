using ItConsultations.Business.Services.ArticleService;
using ItConsultations.Utilities.Guards;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.Access.Articles;

public class ArticleAccessValidationService : AccessValidationServiceBase, IArticleAccessValidationService
{
    private readonly IArticleService _articleService;

    public ArticleAccessValidationService(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public async void ValidateArticleAccessAsync(long id)
    {
        var article = await _articleService.GetByIdAsync(id);

        Guard.NotNull(article);
        Guard.That(article.Title == null, "Article does not have a required title.");
    }
}

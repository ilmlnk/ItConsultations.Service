using ItConsultations.Business.Services.ArticleService;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.AccessValidation.Articles;

public class ArticleAccessValidationService : AccessValidationServiceBase, IArticleAccessValidationService
{
    private readonly IArticleService _articleService;

    public ArticleAccessValidationService(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public void ValidateArticleAccessAsync(string consId)
    {
        //Guard.That(article.Title == null, "Article does not have a required title.");
    }
}

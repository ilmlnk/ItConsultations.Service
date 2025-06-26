using ItConsultations.Business.Dtos.ArticleDtos;

namespace ItConsultations.Business.Services.ArticleService.NormalizeService;

public interface IArticleNormalizationService
{
    Task<ArticleDto> NormalizeAsync(ArticleDto articleDto, string articleConsId);

    Task<ArticleDto> NormalizeResponseAsync(
}

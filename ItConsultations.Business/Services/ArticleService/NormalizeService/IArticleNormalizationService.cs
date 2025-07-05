using ItConsultations.Business.Dtos.ArticleDtos;

namespace ItConsultations.Business.Services.ArticleService.NormalizeService;

public interface IArticleNormalizationService
{
    Task<ArticleDto> NormalizeAsync(ArticleDto articleDto, string articleConsId);

    Task<ArticleDto> NormalizeResponseAsync(ArticleDto articleDto, string language = "en");

    Task<ArticleDto> NormalizeForSearchAsync(ArticleDto articleDto);

    Task<ArticleDto> NormalizeForDisplayAsync(ArticleDto articleDto, string language = "en");
}

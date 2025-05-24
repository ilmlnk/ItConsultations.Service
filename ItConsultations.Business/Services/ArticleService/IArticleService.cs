using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Entities.Article;

namespace ItConsultations.Business.Services.ArticleService;

public interface IArticleService
{
    Task<ArticleDto> CreateAsync(CreateArticleDto dto);

    Task<ArticleDto> DeleteAsync(Article article);

    Task<ArticleDto> GetByIdAsync(long id);

    Task<ArticleDto> GetByIdAsync(string articleConsId);

    Task<List<ArticleDto>> GetAllAsync();
}

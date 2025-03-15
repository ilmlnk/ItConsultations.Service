using ItConsultations.Business.Dtos;
using ItConsultations.Business.Entities.Article;

namespace ItConsultations.Business.Services.ArticleService;

public interface IArticleService
{
    Task<ArticleDto> CreateAsync(ArticleDto dto);

    Task<ArticleDto> UpdateAsync(ArticleDto dto, string articleConsId);

    Task<ArticleDto> DeleteAsync(Article article);

    Task<ArticleDto> GetByIdAsync(long id);

    Task<ArticleDto> GetByIdAsync(string articleConsId);

    Task<List<ArticleDto>> GetAllAsync();
}

using ItConsultations.Business.Dtos.ArticleDtos;

namespace ItConsultations.Business.Services.ArticleService;

public interface IArticleService
{
    Task<ArticleDto> CreateAsync(CreateArticleDto dto, string consId);

    Task DeleteAsync(long id);

    Task DeleteAsync(string articleConsId);

    Task<ArticleDto> GetByIdAsync(long id);

    ArticleDto GetById(string articleConsId);

    Task<List<ArticleDto>> GetAllAsync();
}

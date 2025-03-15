using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos;
using ItConsultations.Business.Entities;
using ItConsultations.Business.Entities.Article;

namespace ItConsultations.Business.Services.ArticleService;

public class ArticleService : IArticleService
{
    private readonly IRepository<Article, long> _repository;
    private readonly IRepository<Entity<long>, long> _entityRepository;

    public ArticleService(
        IRepository<Article, long> repository,
        IRepository<Entity<long>, long> entityRepository
        )
    {
        _repository = repository;
        _entityRepository = entityRepository;
    }

    public async Task<ArticleDto> CreateAsync(ArticleDto dto)
    {
        var user = await _entityRepository.GetAsync(dto.CreatedBy.Id);
        var article = new Article
        {
            ArticleConsId = !string.IsNullOrEmpty(dto.ArticleConsId)
                ? dto.ArticleConsId
                : Guid.NewGuid().ToString("N").Substring(0, 32),
            Title = dto.Title,
            Text = dto.Text,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            CreatedBy = user
        };

        var createdArticle = await _repository.CreateAsync(article);

        return new ArticleDto
        {
            Id = createdArticle.Id,
            ArticleConsId = createdArticle.ArticleConsId,
            Title = createdArticle.Title,
            Text = createdArticle.Text,
            CreatedAt = createdArticle.CreatedAt,
            UpdatedAt = createdArticle.UpdatedAt,
            CreatedBy = createdArticle.CreatedBy
        };
    }

    public async Task<ArticleDto> DeleteAsync(Article article)
    {
        await _repository.DeleteAsync(article);
        var dto = MapperManager.Map<ArticleDto>(article);

        return dto;
    }

    public Task<List<ArticleDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ArticleDto> GetByIdAsync(long id)
    {
        var article = await _repository.GetAsync(id);
        var dto = MapperManager.Map<ArticleDto>(article);

        return dto;
    }

    public Task<ArticleDto> GetByIdAsync(string articleConsId)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto> UpdateAsync(ArticleDto dto, string articleConsId)
    {
        throw new NotImplementedException();
    }
}

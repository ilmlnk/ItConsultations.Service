using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Entities.Article;
using System.Data.Entity;

namespace ItConsultations.Business.Services.ArticleService;

public class ArticleService : IArticleService
{
    private readonly IRepository<Article, long> _repository;

    public ArticleService(
        IRepository<Article, long> repository)
    {
        _repository = repository;
    }

    public async Task<ArticleDto> CreateAsync(CreateArticleDto dto)
    {
        var originalDto = MapperManager.Map<ArticleDto>(dto);
        var article = MapperManager.Map<Article>(originalDto);
        article = await _repository.CreateAsync(article);
        var articleDto = MapperManager.Map<ArticleDto>(article);
        return articleDto;
    }

    public async Task<ArticleDto> DeleteAsync(DeleteArticleDto dto)
    {
        var originalDto = MapperManager.Map<ArticleDto>(dto);
        var article = MapperManager.Map<Article>(originalDto);
        await _repository.DeleteAsync(article);
        return originalDto;
    }

    // add filtering for this method
    public async Task<List<ArticleDto>> GetAllAsync()
    {
        var articles = await _repository.Get(x => true).ToListAsync();
        return MapperManager.Map<List<ArticleDto>>(articles);
    }

    public async Task<ArticleDto> GetByIdAsync(long id)
    {
        var article = await _repository.GetAsync(id);
        var dto = MapperManager.Map<ArticleDto>(article);
        return dto;
    }

    public ArticleDto GetById(string articleConsId)
    {
        var article = _repository.Get(c => c.ArticleConsId.Equals(articleConsId));
        var dto = MapperManager.Map<ArticleDto>(article);
        return dto;
    }
}

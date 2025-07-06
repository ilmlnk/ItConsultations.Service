using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Entities.Article;
using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Utilities.Guards;

namespace ItConsultations.Business.Services.ArticleService;

public class ArticleService : IArticleService
{
    private readonly IRepository<Article, long> _repository;
    private readonly IRepository<Attachment, long> _attachmentRepository;

    public ArticleService(
        IRepository<Article, long> repository,
        IRepository<Attachment, long> attachmentRepository)
    {
        _repository = repository;
        _attachmentRepository = attachmentRepository;
    }

    public async Task<ArticleDto> CreateAsync(CreateArticleDto dto, string consId)
    {
        try
        {
            var article = MapperManager.Map<Article>(dto);
            article.ArticleConsId = GenerateArticleId();
            article.CreatedAt = DateTime.UtcNow;
            article.UpdatedAt = DateTime.UtcNow;
            article = await _repository.CreateAsync(article);
            return MapperManager.Map<ArticleDto>(article);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateAsync: {ex.Message}");
            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw;
        }
    }

    public async Task<ArticleDto> DeleteAsync(DeleteArticleDto dto)
    {
        var article = MapperManager.Map<Article>(dto);
        await _repository.DeleteAsync(article);
        return MapperManager.Map<ArticleDto>(article);
    }

    public async Task<List<ArticleDto>> GetAllAsync()
    {
        var articles = await _repository.GetAllAsync();
        return MapperManager.Map<List<ArticleDto>>(articles);
    }

    public async Task<ArticleDto> GetByIdAsync(long id)
    {
        var article = await _repository.GetAsync(id);
        return article != null ? MapperManager.Map<ArticleDto>(article) : null;
    }

    public ArticleDto GetById(string articleConsId)
    {
        var article = _repository.Get(c => c.ArticleConsId.Equals(articleConsId)).FirstOrDefault();
        return article != null ? MapperManager.Map<ArticleDto>(article) : null;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = _repository.Include(article => article.Attachments)
            .SingleOrDefault(article => article.Id == id);

        Guard.NotNull(entity, nameof(entity));
        Guard.NotNull(entity.Attachments, nameof(entity.Attachments));

        await _attachmentRepository.DeleteAsync(entity.Attachments);
    }

    public async Task DeleteAsync(string articleConsId) 
    {
        var entity = _repository.Include(article => article.Attachments)
            .SingleOrDefault(article => article.ArticleConsId.Equals(articleConsId));

        Guard.NotNull(entity, nameof(entity));
        Guard.NotNull(entity.Attachments, nameof(entity.Attachments));

        await _attachmentRepository.DeleteAsync(entity.Attachments);
        await _repository.DeleteAsync(entity);
    }

    // to generate article id it is used 0006 prefix
    private string GenerateArticleId()
    {
        return $"0006{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";
    }
}

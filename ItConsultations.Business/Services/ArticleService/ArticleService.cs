using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Entities.Articles;
using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Utilities.Guards;
using Microsoft.EntityFrameworkCore;

namespace ItConsultations.Business.Services.ArticleService;

public class ArticleService : IArticleService
{
    private readonly IRepository<Article, long> _repository;
    private readonly IRepository<Attachment, long> _attachmentRepository;
    private readonly IRepository<UserEntity, long> _userRepository;

    public ArticleService(
        IRepository<Article, long> repository,
        IRepository<Attachment, long> attachmentRepository,
        IRepository<UserEntity, long> userRepository)
    {
        _repository = repository;
        _attachmentRepository = attachmentRepository;
        _userRepository = userRepository;
    }

    public async Task<ArticleDto> CreateAsync(CreateArticleDto dto, string userConsId)
    {
        var user = _userRepository
            .Get(u => u.ConsId == userConsId)
            .FirstOrDefault();

        var article = MapperManager.Map<Article>(dto);
        article.ArticleConsId = GenerateArticleId();
        article.CreatedAt = DateTime.UtcNow;
        article.UpdatedAt = DateTime.UtcNow;
        article.CreatedBy = user;
        article = await _repository.CreateAsync(article);
        return MapperManager.Map<ArticleDto>(article);
    }

    public async Task<ArticleDto> DeleteAsync(DeleteArticleDto dto)
    {
        var article = MapperManager.Map<Article>(dto);
        await _repository.DeleteAsync(article);
        return MapperManager.Map<ArticleDto>(article);
    }

    public async Task<List<ArticleDto>> GetAllAsync()
    {
        var articles = await _repository
            .Include(a => a.CreatedBy)
            .Include(a => a.Attachments)
            .ToListAsync();

        return MapperManager.Map<List<ArticleDto>>(articles);
    }

    public async Task<ArticleDto> GetByIdAsync(long id)
    {
        var article = await _repository
            .Include(a => a.CreatedBy)
            .Include(a => a.Attachments)
            .FirstOrDefaultAsync(a => a.Id == id);

        return article != null ? MapperManager.Map<ArticleDto>(article) : null;
    }

    public ArticleDto GetById(string articleConsId)
    {
        var article = _repository
            .Include(a => a.CreatedBy)
            .Include(a => a.Attachments)
            .FirstOrDefault(c => c.ArticleConsId.Equals(articleConsId));

        return article != null ? MapperManager.Map<ArticleDto>(article) : null;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = _repository
            .Include(article => article.Attachments)
            .SingleOrDefault(article => article.Id == id);

        /*Guard.NotNull(entity, nameof(entity));
        Guard.NotNull(entity.Attachments, nameof(entity.Attachments));*/

        await _attachmentRepository.DeleteAsync(entity.Attachments);
    }

    public async Task DeleteAsync(string articleConsId) 
    {
        var entity = _repository
            .Include(article => article.Attachments)
            .SingleOrDefault(article => article.ArticleConsId.Equals(articleConsId));

        /*Guard.NotNull(entity, nameof(entity));
        Guard.NotNull(entity.Attachments, nameof(entity.Attachments));*/

        await _attachmentRepository.DeleteAsync(entity.Attachments);
        await _repository.DeleteAsync(entity);
    }

    public async Task<IEnumerable<ArticleDto>> GetByUserConsIdAsync(string userConsId)
    {
        var articles = await _repository
            .Include(a => a.CreatedBy)
            .Where(a => a.CreatedBy.ConsId == userConsId)
            .ToListAsync();
        
        return MapperManager.Map<IEnumerable<ArticleDto>>(articles);
    }

    // to generate article id it is used 0006 prefix
    private string GenerateArticleId()
    {
        return $"0006{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";
    }
}

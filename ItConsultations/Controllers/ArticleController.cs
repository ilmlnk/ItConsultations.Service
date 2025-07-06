using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Services.ArticleService;
using ItConsultations.Business.Services.Validation.Access.Articles;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.WebApi.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;
    private readonly IArticleAccessValidationService _articleAccessValidationService;
    private readonly ILogger<ArticleController> _logger;

    public ArticleController(
        IArticleService articleService,
        IArticleAccessValidationService articleAccessValidationService,
        ILogger<ArticleController> logger)
    {
        _articleService = articleService;
        _articleAccessValidationService = articleAccessValidationService;
        _logger = logger;
    }

    [HttpPost("create/{consId}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArticleDto dto, string consId)
    {
        try
        {
            //_articleAccessValidationService.ValidateArticleAccessAsync(id);
            var article = await _articleService.CreateAsync(dto, consId);
            
            return Ok(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating article");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("article/{articleConsId}")]
    public async Task<IActionResult> GetAsync(string articleConsId)
    {
        try
        {
            //_articleAccessValidationService.ValidateArticleAccessAsync(id);
            var article = _articleService.GetById(articleConsId);
            
            if (article == null)
            {
                return NotFound($"Article with id {articleConsId} not found");
            }
            
            return Ok(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting article with id: {Id}", articleConsId);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("article/{consId}/{id}")]
    public async Task<IActionResult> GetAsync(long id, string consId)
    {
        try
        {
            //_articleAccessValidationService.ValidateArticleAccessAsync(consId);

            var article = await _articleService.GetByIdAsync(id);

            if (article == null)
            {
                return NotFound($"Article with consId {id} not found");
            }
            
            return Ok(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting article with consId: {ArticleConsId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("get/articles")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var list = await _articleService.GetAllAsync();
            return Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all articles");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("articles/delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteArticleDto[] dtos, long id) 
    {
        try
        {
            //_articleAccessValidationService.ValidateArticleAccessAsync(id);

            foreach (var dto in dtos)
            {
                await _articleService.DeleteAsync(id);
            }
            
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting articles");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("article/delete/{articleConsId}")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteArticleDto dto, long id)
    {
        try
        {
            //_articleAccessValidationService.ValidateArticleAccessAsync(id);
            await _articleService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting article");
            return BadRequest(new { message = ex.Message });
        }
    }
}

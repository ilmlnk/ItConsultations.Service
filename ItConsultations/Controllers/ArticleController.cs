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

    public ArticleController(
        IArticleService articleService,
        IArticleAccessValidationService articleAccessValidationService)
    {
        _articleService = articleService;
        _articleAccessValidationService = articleAccessValidationService;
    }

    [HttpPost("create/{id}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArticleDto dto, long id)
    {
        _articleAccessValidationService.ValidateArticleAccessAsync(id);
        var article = await _articleService.CreateAsync(dto);
        
        return Ok(article);
    }

    [HttpGet("article/{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        _articleAccessValidationService.ValidateArticleAccessAsync(id);
        var article = await _articleService.GetByIdAsync(id);
        // create validator
        return Ok(article);
    }

    [HttpGet("article/{id}/{articleConsId}")]
    public IActionResult Get(long id, string articleConsId)
    {
        _articleAccessValidationService.ValidateArticleAccessAsync(id);

        var article = _articleService.GetById(articleConsId);
        return Ok(article);
    }

    [HttpGet("get/articles")]
    public async Task<IActionResult> GetAllAsync()
    {
        var list = await _articleService.GetAllAsync();
        return Ok(list);
    }

    [HttpDelete("articles/delete/{id}")]
    public async Task DeleteAsync([FromBody] DeleteArticleDto[] dtos, long id) 
    {
        _articleAccessValidationService.ValidateArticleAccessAsync(id);

        foreach (var dto in dtos)
        {
            await _articleService.DeleteAsync(id);
        }
    }

    [HttpDelete("article/delete/{articleConsId}")]
    public async Task DeleteAsync([FromBody] DeleteArticleDto dto, long id)
    {
        _articleAccessValidationService.ValidateArticleAccessAsync(id);
        await _articleService.DeleteAsync(id);
    }
}

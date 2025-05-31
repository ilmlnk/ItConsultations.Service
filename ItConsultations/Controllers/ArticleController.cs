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
        _articleAccessValidationService.ValidateAccessToAdd(id);
        var article = await _articleService.CreateAsync(dto);
        // create normalizer
        return Ok(article);
    }

    [HttpGet("cons/{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        _articleAccessValidationService.ValidateAccessToGet(id);
        var article = await _articleService.GetByIdAsync(id);
        // create validator
        return Ok(article);
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        //_articleAccessValidationService.ValidateAccessToGet(id);
        var article = _articleService.GetById(id);
        // create validator
        return Ok(article);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllAsync()
    {
        var list = await _articleService.GetAllAsync();
        return Ok(list);
    }

    [HttpDelete("delete/{id}")]
    public async Task DeleteAsync([FromBody] DeleteArticleDto[] dtos, long id) 
    {
        _articleAccessValidationService.ValidateAccessToDelete(id);
        // create access validator
        foreach (var dto in dtos)
        {
            await _articleService.DeleteAsync(dto);
        }
    }
}

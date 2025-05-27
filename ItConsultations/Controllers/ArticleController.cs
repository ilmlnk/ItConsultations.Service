using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Services.ArticleService;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.WebApi.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpPost("create/{id}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArticleDto dto, string id)
    {
        var article = _articleService.CreateAsync(dto);
        // create normalizer
        // create validator
        return Ok(article);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
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
    public async Task DeleteAsync([FromBody] DeleteArticleDto[] dtos, string id) 
    {
        // create access validator
        foreach (var dto in dtos)
        {
            await _articleService.DeleteAsync(dto);
        }
    }
}

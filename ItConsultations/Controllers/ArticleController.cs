using ItConsultations.Attributes;
using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Services.ArticleService;
using ItConsultations.Business.Services.Validation.Access.Articles;
using ItConsultations.Business.SharedTypes.Enums.System;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin, UserRole.Coach)]
    [HttpPost("create/{consId}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArticleDto dto, string consId)
    {
        //_articleAccessValidationService.ValidateArticleAccessAsync(id);
        var article = await _articleService.CreateAsync(dto, consId);
        return Ok(article);
    }

    [HttpGet("article/{articleConsId}")]
    public async Task<IActionResult> GetAsync(string articleConsId)
    {
        //_articleAccessValidationService.ValidateArticleAccessAsync(id);
        var article = _articleService.GetById(articleConsId);
        return Ok(article);
    }

    [HttpGet("article/{consId}/{id}")]
    public async Task<IActionResult> GetAsync(long id, string consId)
    {
        //_articleAccessValidationService.ValidateArticleAccessAsync(consId);
        var article = await _articleService.GetByIdAsync(id);
        return Ok(article);
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin, UserRole.Coach)]
    [HttpGet("get/articles")]
    public async Task<IActionResult> GetAllAsync()
    {
        var list = await _articleService.GetAllAsync();
        return Ok(list);
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin, UserRole.Coach)]
    [HttpGet("user/{userConsId}")]
    public async Task<IActionResult> GetByUserConsIdAsync(string userConsId)
    {
        var articles = await _articleService.GetByUserConsIdAsync(userConsId);
        return Ok(articles);
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin, UserRole.Coach)]
    [HttpDelete("articles/delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteArticleDto[] dtos, long id)
    {
        //_articleAccessValidationService.ValidateArticleAccessAsync(id);

        foreach (var dto in dtos)
        {
            await _articleService.DeleteAsync(id);
        }

        return Ok();
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin, UserRole.Coach)]
    [HttpDelete("article/delete/{articleConsId}")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteArticleDto dto, long id)
    {
        //_articleAccessValidationService.ValidateArticleAccessAsync(id);
        await _articleService.DeleteAsync(id);
        return Ok();
    }
}

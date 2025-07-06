using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Services.CoachService;
using ItConsultations.Business.Services.Validation.Access.Coaches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/coaches")]
public class CoachController : Controller
{
    private readonly ICoachService _coachService;
    private readonly ICoachAccessValidationService _validationAccessService;
    private readonly ILogger<CoachController> _logger;

    public CoachController(
        ICoachService coachService,
        ICoachAccessValidationService validationAccessService,
        ILogger<CoachController> logger)
    {
        _coachService = coachService;
        _validationAccessService = validationAccessService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("{coachConsId}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCoachDto dto)
    {
        var createdCoach = await _coachService.CreateAsync(dto);
        return Ok(createdCoach);
    }

    [Authorize]
    [HttpDelete("delete/coach/{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var deletedCoach = await _coachService.DeleteAsync(id);
        return Ok(deletedCoach);
    }

    [Authorize]
    [HttpDelete("delete/coach/consId/{coachConsId}")]
    public async Task<IActionResult> DeleteByConsIdAsync(string coachConsId)
    {
        var coach = _coachService.GetById(coachConsId);
        var deletedCoach = await _coachService.DeleteAsync(coach.Id);
        return Ok(deletedCoach);
    }

    [HttpGet("get/coach/{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var coach = await _coachService.GetAsync(id);
        return Ok(coach);
    }

    [HttpGet("get/coaches")]
    public async Task<IActionResult> GetAllAsync()
    {
        var coaches = await _coachService.GetAllAsync();
        return Ok(coaches);
    }
}

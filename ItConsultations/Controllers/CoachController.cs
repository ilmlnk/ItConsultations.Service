using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Services.CoachService;
using ItConsultations.Business.Services.Validation.AccessValidation.Coaches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.WebApi.Controllers;

[Authorize]
[Route("api/coaches")]
public class CoachController : Controller
{
    private readonly ICoachService _coachService;
    private readonly ICoachAccessValidationService _validationAccessService;

    public CoachController(
        ICoachService coachService,
        ICoachAccessValidationService validationAccessService)
    {
        _coachService = coachService;
        _validationAccessService = validationAccessService;
    }

    [HttpPost("coach")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCoachDto dto)
    {
        var createdCoach = await _coachService.CreateAsync(dto);
        return Ok(createdCoach);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var deletedCoach = await _coachService.DeleteAsync(id);
        return Ok(deletedCoach);
    }

    [HttpDelete("coach/{coachConsId}")]
    public async Task<IActionResult> DeleteAsync(string coachConsId)
    {
        var coach = _coachService.GetCoach(coachConsId);
        var deletedCoach = await _coachService.DeleteAsync(coach.Id);
        return Ok(deletedCoach);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var coach = await _coachService.GetAsync(id);
        return Ok(coach);
    }

    [HttpGet("coach/{coachConsId}")]
    public async Task<IActionResult> Get(string coachConsId)
    {
        var coach = _coachService.GetCoach(coachConsId);
        return Ok(coach);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var coaches = await _coachService.GetAllAsync();
        return Ok(coaches);
    }
}

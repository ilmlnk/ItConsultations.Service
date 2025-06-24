using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Services.CoachService;
using ItConsultations.Business.Services.Validation.Access.Coaches;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

[ApiController]
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

    [HttpPost("{coachConsId}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCoachDto dto)
    {
        var createdCoach = await _coachService.CreateAsync(dto);
        return Ok(createdCoach);
    }

    [HttpDelete("delete/coach/{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        _validationAccessService.ValidateCoachAccessAsync(id);

        var coach = await _coachService.GetAsync(id);
        await _coachService.DeleteAsync(id);
        return Ok(coach);
    }

    [HttpGet("get/coach/{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        _validationAccessService.ValidateCoachAccessAsync(id);

        var coach = await _coachService.GetAsync(id);
        return Ok(coach);
    }

    [HttpGet("get/coaches")]
    public async Task<IActionResult> GetAllAsync()
    {
        var coaches = await _coachService.GetAllAsync();
        return Ok(coaches);
    }

    [HttpDelete("coach/{id}/{coachConsId}")]
    public async Task<IActionResult> DeleteAsync(long id, string coachConsId)
    {
        _validationAccessService.ValidateCoachAccessAsync(id);
        var coach = _coachService.GetById(coachConsId);
        await _coachService.DeleteAsync(coach.Id);
        return Ok(coach);
    }
}

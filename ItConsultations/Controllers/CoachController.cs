using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Services.CoachService;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/coaches")]
public class CoachController : Controller
{
    private readonly ICoachService _coachService;

    public CoachController(
        ICoachService coachService)
    {
        _coachService = coachService;
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
        var coach = await _coachService.GetAsync(id);
        // create access validator
        await _coachService.DeleteAsync(id);
        return Ok(coach);
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

    [HttpDelete("coach/{coachConsId}")]
    public async Task<IActionResult> DeleteAsync(string coachConsId)
    {
        var coach = _coachService.GetById(coachConsId);
        await _coachService.DeleteAsync(coach.Id);
        // create access validator
        return Ok(coach);
    }
}

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

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCoachDto dto)
    {
        var createdCoach = await _coachService.CreateAsync(dto);
        return Ok(createdCoach);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var coach = _coachService.GetAsync(id);
        // create access validator
        await _coachService.DeleteAsync(id);
        return Ok(coach);
    }

    [HttpDelete("coach/{coachConsId}")]
    public async Task<IActionResult> DeleteAsync(string coachConsId)
    {
        var coach = _coachService.GetById(coachConsId);
        // create access validator
        return Ok(coach);
    }
}

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
    public async Task<ActionResult<CoachDto>> CreateAsync([FromBody] CreateCoachDto dto)
    {
        var createdCoach = await _coachService.CreateAsync(dto);
        return Ok(createdCoach);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<CoachDto>> DeleteAsync(long id)
    {
        try
        {
            await _coachService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound($"Coach with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpDelete("delete/{coachConsId}")]
    public async Task<ActionResult> DeleteAsync(string coachConsId)
    {
        try
        {
            var coach = _coachService.Get(coachConsId);
            await _coachService.DeleteAsync(coach.Id);

            return Ok();
        }
        catch
        {
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}

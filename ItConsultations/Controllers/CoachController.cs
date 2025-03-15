using ItConsultations.Business.Dtos;
using ItConsultations.Business.Services.Coach;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

public class CoachController : Controller
{
    private readonly ICoachService _coachService;

    public CoachController(
        ICoachService coachService
        )
    {
        _coachService = coachService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<CoachDto>> CreateAsync([FromBody] CoachDto dto)
    {
        var createdCoach = await _coachService.CreateAsync(dto);
        return Ok(createdCoach);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<CoachDto>> DeleteAsync(long id)
    {
        try
        {
            await _coachService.DeleteAsync(id);
            return NoContent();
        } catch (KeyNotFoundException e)
        {
            return NotFound($"Coach with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    public async Task<ActionResult<CoachDto>> GetAllAsync()
    {
        try
        {
            var coaches = await _coachService.GetAllAsync();
            return Ok(coaches);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    public Task<CoachDto> UpdateAsync(CoachDto dto, string id)
    {

    }
}

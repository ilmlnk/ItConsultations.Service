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

    [HttpPost("{coachConsId}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCoachDto dto)
    {
        try
        {
            //_logger.LogInformation("Creating coach with consId: {CoachConsId}", dto?.CoachConsId);
            var createdCoach = await _coachService.CreateAsync(dto);
            return Ok(createdCoach);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating coach");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/coach/{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        try
        {
            _logger.LogInformation("Deleting coach with id: {Id}", id);
            
            var deletedCoach = await _coachService.DeleteAsync(id);
            if (deletedCoach == null)
            {
                _logger.LogWarning("Coach with id {Id} not found", id);
                return NotFound($"Coach with id {id} not found");
            }

            _logger.LogInformation("Successfully deleted coach with id: {Id}", id);
            return Ok(deletedCoach);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting coach with id: {Id}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/coach/consId/{coachConsId}")]
    public async Task<IActionResult> DeleteByConsIdAsync(string coachConsId)
    {
        try
        {
            _logger.LogInformation("Deleting coach with consId: {CoachConsId}", coachConsId);
            
            var coach = _coachService.GetById(coachConsId);
            if (coach == null)
            {
                _logger.LogWarning("Coach with consId {CoachConsId} not found", coachConsId);
                return NotFound($"Coach with consId {coachConsId} not found");
            }
            
            var deletedCoach = await _coachService.DeleteAsync(coach.Id);
            _logger.LogInformation("Successfully deleted coach with consId: {CoachConsId}", coachConsId);
            return Ok(deletedCoach);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting coach with consId: {CoachConsId}", coachConsId);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("get/coach/{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        try
        {
            _logger.LogInformation("Getting coach with id: {Id}", id);
            
            var coach = await _coachService.GetAsync(id);
            if (coach == null)
            {
                _logger.LogWarning("Coach with id {Id} not found", id);
                return NotFound($"Coach with id {id} not found");
            }

            return Ok(coach);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting coach with id: {Id}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("get/coaches")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Getting all coaches");
            var coaches = await _coachService.GetAllAsync();
            return Ok(coaches);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all coaches");
            return BadRequest(new { message = ex.Message });
        }
    }
}

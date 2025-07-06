using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Services.ConsultationService;
using ItConsultations.Business.Services.Validation.Access.Consultations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;


[ApiController]
[Route("api/consultations")]
public class ConsultationController : Controller
{
    private readonly IConsultationService _consultationService;
    private readonly IConsultationAccessValidationService _accessValidationService;

    public ConsultationController(
        IConsultationService consultationService,
        IConsultationAccessValidationService accessValidationService) 
    {
        _consultationService = consultationService;
        _accessValidationService = accessValidationService;
    }

    [Authorize]
    [HttpPost("consultation/{coachConsId}")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateConsultationDto dto, string coachConsId)
    {
        var consultation = await _consultationService.CreateAsync(dto, coachConsId);
        // create access validator
        return Ok(consultation);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var consultation = await _consultationService.GetAsync(id);
        // create access validator
        return Ok(consultation);
    }

    [HttpGet("consultation/{consId}")]
    public async Task<IActionResult> GetAsync(string consId)
    {
        var consultation = await _consultationService.GetAsync(consId);
        // add access validator
        return Ok(consultation);
    }

    [Authorize]
    [HttpPut("consultation/{id}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateConsultationDto dto, long id)
    {
        var consultation = await _consultationService.UpdateAsync(dto, id);
        // create access validator
        return Ok(consultation);
    }

    [HttpGet("consultation-list")]
    public async Task<IActionResult> GetAllAsync()
    {
        var consultations = await _consultationService.GetAsync();
        // create access validator
        return Ok(consultations);
    }

    [Authorize]
    [HttpDelete("consultations/{id}/{consId}")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteConsultationDto dto, long id)
    {
        _accessValidationService.ValidateConsultationAccessAsync(id, dto.ConsId);
        await _consultationService.DeleteAsync(dto, id);
        return Ok();
    }

    [Authorize]
    [HttpDelete("consultations/{id}")]
    public async Task<IActionResult> DeleteForUserAsync([FromBody] DeleteConsultationDto dto, long id)
    {
        _accessValidationService.ValidateConsultationAccessAsync(id, dto.ConsId);
        await _consultationService.DeleteForUserAsync(dto, id);
        return Ok();
    }
}

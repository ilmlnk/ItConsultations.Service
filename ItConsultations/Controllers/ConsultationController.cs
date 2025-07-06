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
    public async Task<IActionResult> GetByConsIdAsync(string consId)
    {
        var consultation = await _consultationService.GetAsync(consId);
        // add access validator
        return Ok(consultation);
    }

    [HttpGet("consultation/coach/{coachConsId}")]
    public async Task<IActionResult> GetByCoachConsIdAsync(string coachConsId)
    {
        var consultations = await _consultationService.GetByCoachConsIdAsync(coachConsId);
        return Ok(consultations);
    }

    [Authorize]
    [HttpPut("consultation/{id}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateConsultationDto dto, long id)
    {
        var consultation = await _consultationService.UpdateAsync(dto, id);
        // create access validator
        return Ok(consultation);
    }

    [HttpGet("consultations")]
    public async Task<IActionResult> GetAllAsync()
    {
        var consultations = await _consultationService.GetAllAsync();
        // create access validator
        return Ok(consultations);
    }

    [Authorize]
    [HttpDelete("consultations/coach/{coachConsId}")]
    public async Task<IActionResult> DeleteAsync(string coachConsId)
    {
        //_accessValidationService.ValidateConsultationAccessAsync(id, dto.ConsId);
        await _consultationService.DeleteForUserAsync(coachConsId);
        return Ok();
    }

    /*[Authorize]
    [HttpDelete("consultations/delete")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteConsultationDto dto)
    {
        //_accessValidationService.ValidateConsultationAccessAsync(id, dto.ConsId);
        await _consultationService.DeleteForUserAsync(dto);
        return Ok();
    }*/

    [Authorize]
    [HttpDelete("consultations/consId/{consId}")]
    public async Task<IActionResult> DeleteByConsIdAsync(string consId)
    {
        //_accessValidationService.ValidateConsultationAccessAsync(id, dto.ConsId);
        await _consultationService.DeleteAsync(consId);
        return Ok();
    }

    [Authorize]
    [HttpDelete("consultations/{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        await _consultationService.DeleteAsync(id);
        return Ok();
    }
}

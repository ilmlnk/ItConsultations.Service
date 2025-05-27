using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Business.Services.ConsultationService;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;


[ApiController]
[Route("api/consultations")]
public class ConsultationController : Controller
{
    private readonly IRepository<Consultation, long> _consultationRepository;
    private readonly IConsultationService _consultationService;

    public ConsultationController(
        IRepository<Consultation, long> consultationRepository,
        IConsultationService consultationService) 
    {
        _consultationRepository = consultationRepository;
        _consultationService = consultationService;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateConsultationDto dto, string consId)
    {
        var consultation = await _consultationService.CreateAsync(dto);
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

    [HttpPut("")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateConsultationDto dto, long id)
    {
        var consultation = await _consultationService.UpdateAsync(dto, id);
        // create access validator
        return Ok(consultation);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync()
    {
        var consultations = await _consultationService.GetAsync();
        // create access validator
        return Ok(consultations);
    }
}

using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Entities.Consultation;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;


[ApiController]
[Route("api/consultations")]
public class ConsultationController : Controller
{
    private readonly IRepository<Consultation, long> _consultationRepository;

    public ConsultationController(
        IRepository<Consultation, long> consultationRepository) 
    {
        _consultationRepository = consultationRepository;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateConsultationDto dto, string consId)
    {

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync()
    {

    }

    [HttpPut("")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateConsultationDto dto, string consId)
    {

    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync()
    {

    }
}

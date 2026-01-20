using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.OpenApi.Controllers.Configs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ItConsultations.OpenApi.Controllers;

[ApiVersion(ApiVersions.ApiVersionV1)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ConsultationsController : ControllerBase
{
    public ConsultationsController()
    {

    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<ConsultationDto>), (int)HttpStatusCode.OK)]
    public IActionResult GetConsultations()
    {
        var consultations = new List<ConsultationDto>();
        return Ok(consultations);
    }
}

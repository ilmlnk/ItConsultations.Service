using ItConsultations.Business.Entities.Consultation;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;


[Route("api/consultations")]
[ApiController]
public class ConsultationController : Controller
{
    private static List<Consultation> _consultations = [];

    public ConsultationController() { }

    /*public IActionResult CreateConsultation([FromBody] ConsultationDto consultationDto)
    {
        if (consultationDto == null)
        {
            return BadRequest("Invalid consultation data.");
        }

        var consultation = new Consultation
        {
            Id = consultationDto.Id,

        }
    }*/
}

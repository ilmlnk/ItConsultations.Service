using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Business.Services.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

[Authorize]
[Route("api/students")]
public class StudentController : Controller
{
    private readonly IStudentService _studentService;

    public StudentController(
        IStudentService studentService
        )
    {
        _studentService = studentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateStudentDto dto, string id)
    {   
        // create access validator
        var student = await _studentService.CreateAsync(dto, id);
        return Ok(student);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student == null ? NotFound() : Ok(student);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllAsync()
    {
        // create access validator
        return null;
    }


}

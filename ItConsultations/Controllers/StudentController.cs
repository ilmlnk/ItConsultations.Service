using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Services.StudentService;
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
    public async Task<IActionResult> GetAsync(long id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student == null ? NotFound() : Ok(student);
    }

    [HttpGet("get/students")]
    public async Task<IActionResult> GetAllAsync()
    {
        var students = await _studentService.GetAllAsync();
        // create access validator
        return Ok(students);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(string studentConsId)
    {
        return Ok(DeleteAsync(studentConsId));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        await _studentService.DeleteAsync(id);
        return Ok();
    }
}

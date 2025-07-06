using ItConsultations.Attributes;
using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Services.StudentService;
using ItConsultations.Business.SharedTypes.Enums.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

[Authorize]
[Route("api/students")]
public class StudentController : Controller
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateStudentDto dto)
    {   
        // create access validator
        var student = await _studentService.CreateAsync(dto);
        return Ok(student);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student == null ? NotFound() : Ok(student);
    }

    [HttpGet("studentConsId/{studentConsId}")]
    public async Task<IActionResult> GetByConsIdAsync(string studentConsId)
    {
        var student = await _studentService.GetByIdAsync(studentConsId);
        return student == null ? NotFound() : Ok(student);
    }

    [HttpGet("get/students")]
    public async Task<IActionResult> GetAllAsync()
    {
        var students = await _studentService.GetAllAsync();
        // create access validator
        return Ok(students);
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        await _studentService.DeleteAsync(id);
        return Ok();
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin)]
    [HttpDelete("delete/studentConsId/{studentConsId}")]
    public async Task<IActionResult> DeleteAsync(string studentConsId)
    {
        await _studentService.DeleteAsync(studentConsId);
        return Ok();
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin)]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] UpdateStudentDto dto)
    {
        var student = await _studentService.UpdateAsync(dto, id);
        return student == null ? NotFound() : Ok(student);
    }

    [Authorize]
    [AuthorizeRoles(UserRole.Student, UserRole.Admin)]
    [HttpPut("update/studentConsId/{studentConsId}")]
    public async Task<IActionResult> UpdateByConsIdAsync(string studentConsId, [FromBody] UpdateStudentDto dto)
    {
        var student = await _studentService.UpdateAsync(dto, studentConsId);
        return student == null ? NotFound() : Ok(student);
    }
}

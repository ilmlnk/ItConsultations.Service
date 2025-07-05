using ItConsultations.Attributes;
using ItConsultations.Business.SharedTypes.Enums.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/TestController")]
public class TestController : ControllerBase
{
    [HttpGet("student-only")]
    [AuthorizeRoles(UserRole.Student)]
    public IActionResult StudentOnly()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "This endpoint is accessible only to students",
            userId,
            userEmail,
            userRole
        });
    }

    [HttpGet("coach-only")]
    [AuthorizeRoles(UserRole.Coach)]
    public IActionResult CoachOnly()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "This endpoint is accessible only to coaches",
            userId,
            userEmail,
            userRole
        });
    }

    [HttpGet("admin-only")]
    [AuthorizeRoles(UserRole.Admin)]
    public IActionResult AdminOnly()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "This endpoint is accessible only to administrators",
            userId,
            userEmail,
            userRole
        });
    }

    [HttpGet("student-coach")]
    [AuthorizeRoles(UserRole.Student, UserRole.Coach)]
    public IActionResult StudentAndCoach()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "This endpoint is accessible to students and coaches",
            userId,
            userEmail,
            userRole
        });
    }

    [HttpGet("authenticated")]
    [Authorize]
    public IActionResult Authenticated()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "This endpoint is accessible to all authenticated users",
            userId,
            userEmail,
            userRole
        });
    }

    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new
        {
            message = "This endpoint is accessible to everyone",
            timestamp = DateTime.UtcNow
        });
    }
} 
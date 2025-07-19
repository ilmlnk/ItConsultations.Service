using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Services.AuthService;
using ItConsultations.Business.SharedTypes.Enums.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly IConfiguration _configuration;

    public AuthController(IFirebaseAuthService firebaseAuthService, IConfiguration configuration)
    {
        _firebaseAuthService = firebaseAuthService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var userInfo = await _firebaseAuthService.RegisterAsync(registerDto);
        return Ok(userInfo);
    }

    [HttpPost("register-simple")]
    public IActionResult RegisterSimple([FromBody] RegisterRequest request)
    {
        var token = GenerateJwtToken(request.Username, request.Role);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var token = GenerateJwtToken(request.Username, request.Role);
        return Ok(new { token });
    }

    [HttpPost("token/{role}")]
    public IActionResult GetTokenWithRole(string role, [FromBody] LoginRequest request)
    {
        var token = GenerateJwtToken(request.Username, role);
        return Ok(new { token, role });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var result = await _firebaseAuthService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
    {
        var userId = User.FindFirst("firebase_uid")?.Value;
        var result = await _firebaseAuthService.RevokeTokenAsync(logoutDto.RefreshToken, userId);
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var firebaseUid = User.FindFirst("firebase_uid")?.Value;
        var userInfo = await _firebaseAuthService.GetUserInfoAsync(firebaseUid);
        return Ok(userInfo);
    }

    [HttpGet("firebase-status")]
    public IActionResult GetFirebaseStatus()
    {
        var status = new
        {
            isInitialized = FirebaseApp.DefaultInstance != null,
            projectId = FirebaseApp.DefaultInstance?.Options?.ProjectId,
            message = FirebaseApp.DefaultInstance != null 
                ? "Firebase is properly initialized" 
                : "Firebase is not initialized"
        };

        return Ok(status);
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenDto validateTokenDto)
    {
        var isValid = await _firebaseAuthService.ValidateTokenAsync(validateTokenDto.AccessToken);
        return Ok(new { isValid });
    }

    private string GenerateJwtToken(string username, string role)
    {
        var jwtSecret = _configuration["Jwt:Secret"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateJwtToken(string username)
    {
        return GenerateJwtToken(username, "Student");
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
} 
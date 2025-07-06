using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/AuthController")]
public class AuthController : ControllerBase
{
    private readonly IFirebaseAuthService _firebaseAuthService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IFirebaseAuthService firebaseAuthService, ILogger<AuthController> logger)
    {
        _firebaseAuthService = firebaseAuthService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var tokenParts = registerDto.IdToken.Split('.');
            var userInfo = await _firebaseAuthService.RegisterAsync(registerDto);
            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", registerDto?.Email);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {

            var tokenParts = loginDto.IdToken.Split('.');
            var result = await _firebaseAuthService.LoginAsync(loginDto.IdToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        try {
            var result = await _firebaseAuthService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
    {
        try
        {

            var userId = User.FindFirst("firebase_uid")?.Value;
            var result = await _firebaseAuthService.RevokeTokenAsync(logoutDto.RefreshToken, userId);
            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var firebaseUid = User.FindFirst("firebase_uid")?.Value;

            var userInfo = await _firebaseAuthService.GetUserInfoAsync(firebaseUid);

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("firebase-status")]
    public IActionResult GetFirebaseStatus()
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking Firebase status");
            return BadRequest(new { 
                message = "Error checking Firebase status",
                details = ex.Message
            });
        }
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenDto validateTokenDto)
    {
        try
        {
            var isValid = await _firebaseAuthService.ValidateTokenAsync(validateTokenDto.AccessToken);
            return Ok(new { isValid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return BadRequest(new { message = ex.Message });
        }
    }
} 
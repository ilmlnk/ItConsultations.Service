using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Services.AuthService;
using ItConsultations.Business.Guards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/AuthController")]
public class AuthController : ControllerBase
{
    private readonly IFirebaseAuthService _firebaseAuthService;

    public AuthController(IFirebaseAuthService firebaseAuthService)
    {
        _firebaseAuthService = firebaseAuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            Guard.NotNull(registerDto, nameof(registerDto));
            Guard.NotNullOrEmpty(registerDto.IdToken, nameof(registerDto.IdToken));

            var userInfo = await _firebaseAuthService.RegisterAsync(registerDto);
            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            Guard.NotNull(loginDto, nameof(loginDto));
            Guard.NotNullOrEmpty(loginDto.IdToken, nameof(loginDto.IdToken));

            var result = await _firebaseAuthService.LoginAsync(loginDto.IdToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        try
        {
            Guard.NotNull(refreshTokenDto, nameof(refreshTokenDto));
            Guard.NotNullOrEmpty(refreshTokenDto.RefreshToken, nameof(refreshTokenDto.RefreshToken));

            var result = await _firebaseAuthService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
    {
        try
        {
            Guard.NotNull(logoutDto, nameof(logoutDto));
            Guard.NotNullOrEmpty(logoutDto.RefreshToken, nameof(logoutDto.RefreshToken));

            var userId = User.FindFirst("firebase_uid")?.Value;
            Guard.NotNull(userId, nameof(userId));

            var result = await _firebaseAuthService.RevokeTokenAsync(logoutDto.RefreshToken, userId);
            Guard.True(result, nameof(result));
            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
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
            Guard.NotNull(firebaseUid, nameof(firebaseUid));

            var userInfo = await _firebaseAuthService.GetUserInfoAsync(firebaseUid);
            Guard.NotNull(userInfo, nameof(userInfo));

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenDto validateTokenDto)
    {
        try
        {
            Guard.NotNull(validateTokenDto, nameof(validateTokenDto));
            Guard.NotNullOrEmpty(validateTokenDto.AccessToken, nameof(validateTokenDto.AccessToken));

            var isValid = await _firebaseAuthService.ValidateTokenAsync(validateTokenDto.AccessToken);
            return Ok(new { isValid });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 
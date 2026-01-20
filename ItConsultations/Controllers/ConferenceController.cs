using System.Security.Claims;
using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Dtos.ConferenceDtos.RecordingDtos;
using ItConsultations.Business.Services.ConferenceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.WebApi.Controllers;

[Route("api/conferences")]
[Authorize]
public class ConferenceController : ControllerBase
{
    private readonly IConferenceService _conferenceService;
    private readonly ILogger<ConferenceController> _logger;

    public ConferenceController(IConferenceService conferenceService, ILogger<ConferenceController> logger)
    {
        _conferenceService = conferenceService;
        _logger = logger;
    }

    #region Conference Management

    [HttpPost]
    public async Task<IActionResult> CreateConference([FromBody] CreateConferenceDto dto)
    {
        var userId = GetCurrentUserId();
        var conference = await _conferenceService.CreateConferenceAsync(dto);
        return CreatedAtAction(nameof(GetConference), new { id = conference.Id }, conference);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConference(long id, [FromBody] UpdateConferenceDto dto)
    {
        var conference = await _conferenceService.UpdateConferenceAsync(id, dto);
        return Ok(conference);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConference(long id)
    {
        var result = await _conferenceService.DeleteConferenceAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetConference(long id)
    {
        try
        {
            var conference = await _conferenceService.GetConferenceAsync(id);

            if (conference == null)
            {
                return NotFound("Conference not found");
            }

            return Ok(conference);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when getting conference");
        }
    }

    [HttpGet("cons/{consId}")]
    public async Task<IActionResult> GetConferenceByConsId(string consId)
    {
        try
        {
            var conference = await _conferenceService.GetConferenceAsync(consId);

            if (conference == null)
            {
                return NotFound("Conference not found");
            }

            return Ok(conference);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conference by consId {ConsId}", consId);
            return StatusCode(500, "Internal server error when getting conference");
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyConferences([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var conferences = await _conferenceService.GetUserConferencesAsync(userId, fromDate, toDate);
            return Ok(conferences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user conferences");
            return StatusCode(500, "Internal server error when getting user conferences");
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchConferences([FromQuery] ConferenceSearchDto searchDto)
    {
        try
        {
            var conferences = await _conferenceService.SearchConferencesAsync(searchDto);
            return Ok(conferences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching conferences");
            return StatusCode(500, "Internal server error when searching conferences");
        }
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingConferences([FromQuery] int days = 7)
    {
        try
        {
            var conferences = await _conferenceService.GetUpcomingConferencesAsync(days);
            return Ok(conferences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting upcoming conferences");
            return StatusCode(500, "Internal server error when getting upcoming conferences");
        }
    }

    #endregion

    #region Participant Management

    [HttpPost("{id}/join")]
    public async Task<IActionResult> JoinConference([FromBody] JoinConferenceDto joinDto, string id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var conference = await _conferenceService.JoinConferenceAsync(joinDto, id);

            if (conference == null)
            {
                return NotFound("Conference not found or access denied");
            }

            _logger.LogInformation("User {UserId} joined conference {ConferenceId}", userId, id);
            return Ok(conference);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid password or access denied");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error joining conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when joining conference");
        }
    }

    [HttpPost("{id}/leave")]
    public async Task<IActionResult> LeaveConference(string id)
    {
        try
        {
            var userId = GetCurrentUserId();

            var result = await _conferenceService.LeaveConferenceAsync(id, userId);

            _logger.LogInformation("User {UserId} left conference {ConferenceId}", userId, id);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error leaving conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when leaving conference");
        }
    }

    [HttpPost("{id}/participants")]
    public async Task<IActionResult> AddParticipant(string id, [FromBody] AddParticipantDto dto)
    {
        try
        {
            var result = await _conferenceService.AddParticipantAsync(id, dto);

            if (result == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Participant {UserId} added to conference {ConferenceId}", dto.UserId, id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding participant to conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when adding participant");
        }
    }

    [HttpDelete("{id}/participants/{userId}")]
    public async Task<IActionResult> RemoveParticipant(string id, long userId)
    {
        try
        {
            var result = await _conferenceService.RemoveParticipantAsync(id, userId);
            _logger.LogInformation("Participant {UserId} removed from conference {ConferenceId}", userId, id);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing participant from conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when removing participant");
        }
    }

    [HttpPut("{id}/participants/{userId}/role")]
    public async Task<IActionResult> UpdateParticipantRole([FromBody] UpdateParticipantRoleDto dto, string id, long userId)
    {
        try
        {
            var result = await _conferenceService.UpdateParticipantRoleAsync(dto, id, userId);

            if (result == null)
            {
                return NotFound("Conference or participant not found");
            }

            _logger.LogInformation("Participant {UserId} role updated in conference {ConferenceId}", userId, id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating participant role in conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when updating participant role");
        }
    }

    #endregion

    #region Recording Management

    [HttpPost("{id}/recording")]
    public async Task<IActionResult> ActivateRecording(string id, [FromBody] ActivateRecordingDto dto)
    {
        try
        {
            var result = await _conferenceService.ActivateRecordingAsync(id, dto.EnableRecording, dto.EnableChatRecording);

            if (!result)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Recording settings updated for conference {ConferenceId}", id);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating recording for conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when activating recording");
        }
    }

    [HttpPost("{id}/recording/upload")]
    public async Task<IActionResult> UploadRecording(string id, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var recording = await _conferenceService.UploadRecordingAsync(id, fileBytes, file.FileName);

            if (recording == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Recording uploaded for conference {ConferenceId}", id);
            return Ok(recording);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading recording for conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when uploading recording");
        }
    }

    [HttpPost("{id}/chat/upload")]
    public async Task<IActionResult> UploadChatLog(string id, IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        var recording = await _conferenceService.UploadChatLogAsync(id, fileBytes, file.FileName);
        return Ok(recording);
    }

    [HttpGet("recording/{recordingId}/download")]
    public async Task<IActionResult> DownloadRecording(long recordingId)
    {
        var fileBytes = await _conferenceService.DownloadRecordingAsync(recordingId);
        return File(fileBytes, "application/octet-stream", "recording.mp4");
    }

    [HttpGet("chat/{recordingId}/download")]
    public async Task<IActionResult> DownloadChatLog(long recordingId)
    {
        var fileBytes = await _conferenceService.DownloadChatLogAsync(recordingId);
        return File(fileBytes, "application/octet-stream", "chatlog.txt");
    }

    [HttpDelete("recording/{recordingId}")]
    public async Task<IActionResult> DeleteRecording(long recordingId)
    {
        var result = await _conferenceService.DeleteRecordingAsync(recordingId);
        return Ok(new { success = true });
    }

    #endregion

    #region Conference Actions

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartConference(string id)
    {
        var result = await _conferenceService.StartConferenceAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/end")]
    public async Task<IActionResult> EndConference(string id)
    {
        var result = await _conferenceService.EndConferenceAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/pause")]
    public async Task<IActionResult> PauseConference(string id)
    {
        var result = await _conferenceService.PauseConferenceAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/resume")]
    public async Task<IActionResult> ResumeConference(string id)
    {
        var result = await _conferenceService.ResumeConferenceAsync(id);
        return Ok(result);
    }

    #endregion

    #region Statistics

    [HttpGet("{id}/statistics")]
    public async Task<IActionResult> GetConferenceStatistics(string id)
    {
        var statistics = await _conferenceService.GetConferenceStatisticsAsync(id);
        return Ok(statistics);
    }

    #endregion

    private string GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        /*if (long.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }*/

        // Fallback to a default user ID for development
        return "";
    }
}

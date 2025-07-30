using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItConsultations.Business.Dtos.ConferenceDtos.RecordingDtos;
using System.Security.Claims;
using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Services.ConferenceService;

namespace ItConsultations.Controllers;

[ApiController]
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
        try
        {
            var userId = GetCurrentUserId();
            var conference = await _conferenceService.CreateConferenceAsync(dto);
            
            _logger.LogInformation("Conference {ConferenceId} created by user {UserId}", conference.Id, userId);
            return CreatedAtAction(nameof(GetConference), new { id = conference.Id }, conference);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating conference");
            return StatusCode(500, "Internal server error when creating conference");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConference(long id, [FromBody] UpdateConferenceDto dto)
    {
        try
        {
            var conference = await _conferenceService.UpdateConferenceAsync(id, dto);

            if (conference == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Conference {ConferenceId} updated", id);
            return Ok(conference);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when updating conference");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConference(long id)
    {
        try
        {
            var result = await _conferenceService.DeleteConferenceAsync(id);

            if (!result)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Conference {ConferenceId} deleted", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when deleting conference");
        }
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
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var recording = await _conferenceService.UploadChatLogAsync(id, fileBytes, file.FileName);

            if (recording == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Chat log uploaded for conference {ConferenceId}", id);
            return Ok(recording);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading chat log for conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when uploading chat log");
        }
    }

    [HttpGet("recording/{recordingId}/download")]
    public async Task<IActionResult> DownloadRecording(long recordingId)
    {
        try
        {
            var fileBytes = await _conferenceService.DownloadRecordingAsync(recordingId);

            if (fileBytes == null)
            {
                return NotFound("Recording not found");
            }

            return File(fileBytes, "application/octet-stream", "recording.mp4");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading recording {RecordingId}", recordingId);
            return StatusCode(500, "Internal server error when downloading recording");
        }
    }

    [HttpGet("chat/{recordingId}/download")]
    public async Task<IActionResult> DownloadChatLog(long recordingId)
    {
        try
        {
            var fileBytes = await _conferenceService.DownloadChatLogAsync(recordingId);

            if (fileBytes == null)
            {
                return NotFound("Chat log not found");
            }

            return File(fileBytes, "application/octet-stream", "chatlog.txt");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading chat log {RecordingId}", recordingId);
            return StatusCode(500, "Internal server error when downloading chat log");
        }
    }

    [HttpDelete("recording/{recordingId}")]
    public async Task<IActionResult> DeleteRecording(long recordingId)
    {
        try
        {
            var result = await _conferenceService.DeleteRecordingAsync(recordingId);
            _logger.LogInformation("Recording {RecordingId} deleted", recordingId);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting recording {RecordingId}", recordingId);
            return StatusCode(500, "Internal server error when deleting recording");
        }
    }

    #endregion

    #region Conference Actions

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartConference(string id)
    {
        try
        {
            var result = await _conferenceService.StartConferenceAsync(id);

            if (result == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Conference {ConferenceId} started", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when starting conference");
        }
    }

    [HttpPost("{id}/end")]
    public async Task<IActionResult> EndConference(string id)
    {
        try
        {
            var result = await _conferenceService.EndConferenceAsync(id);

            if (result == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Conference {ConferenceId} ended", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ending conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when ending conference");
        }
    }

    [HttpPost("{id}/pause")]
    public async Task<IActionResult> PauseConference(string id)
    {
        try
        {
            var result = await _conferenceService.PauseConferenceAsync(id);

            if (result == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Conference {ConferenceId} paused", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pausing conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when pausing conference");
        }
    }

    [HttpPost("{id}/resume")]
    public async Task<IActionResult> ResumeConference(string id)
    {
        try
        {
            var result = await _conferenceService.ResumeConferenceAsync(id);

            if (result == null)
            {
                return NotFound("Conference not found");
            }

            _logger.LogInformation("Conference {ConferenceId} resumed", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resuming conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when resuming conference");
        }
    }

    #endregion

    #region Statistics

    [HttpGet("{id}/statistics")]
    public async Task<IActionResult> GetConferenceStatistics(string id)
    {
        try
        {
            var statistics = await _conferenceService.GetConferenceStatisticsAsync(id);

            if (statistics == null)
            {
                return NotFound("Conference not found");
            }

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting statistics for conference {ConferenceId}", id);
            return StatusCode(500, "Internal server error when getting conference statistics");
        }
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

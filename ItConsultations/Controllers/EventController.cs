using ItConsultations.Business.Dtos.AttachmentDtos;
using ItConsultations.Business.Dtos.EventDtos;
using ItConsultations.Business.Services.EventService;
using ItConsultations.Business.SharedTypes.Enums.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _eventService.CreateAsync(createDto, userId);
            
            _logger.LogInformation("Event {EventId} created by user {UserId}", result.Id, userId);
            return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error when creating event");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, "Internal server error when creating event");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(long id, [FromBody] UpdateEventDto updateDto)
    {
        try
        {
            updateDto.Id = id;
            var result = await _eventService.UpdateAsync(updateDto);
            
            _logger.LogInformation("Event {EventId} updated", id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error when updating event {EventId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}", id);
            return StatusCode(500, "Internal server error when updating event");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(long id)
    {
        try
        {
            var result = await _eventService.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("Event {EventId} deleted", id);
                return Ok(result);
            }
            else
            {
                return NotFound("Event not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event {EventId}", id);
            return StatusCode(500, "Internal server error when deleting event");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(long id)
    {
        try
        {
            var result = await _eventService.GetByIdAsync(id);
            
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Event not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event {EventId}", id);
            return StatusCode(500, "Internal server error when getting event");
        }
    }

    [HttpGet("cons/{consId}")]
    public async Task<IActionResult> GetEventByConsId(string consId)
    {
        try
        {
            var result = await _eventService.GetByConsIdAsync(consId);
            
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Event not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event by consId {ConsId}", consId);
            return StatusCode(500, "Internal server error when getting event");
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchEvents([FromQuery] EventSearchDto searchDto)
    {
        try
        {
            var result = await _eventService.SearchAsync(searchDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching events");
            return StatusCode(500, "Internal server error when searching events");
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserEvents([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _eventService.GetUserEventsAsync(userId, fromDate, toDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user events");
            return StatusCode(500, "Internal server error when getting user events");
        }
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingEvents([FromQuery] int days = 7)
    {
        try
        {
            var result = await _eventService.GetUpcomingEventsAsync(days);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting upcoming events");
            return StatusCode(500, "Internal server error when getting upcoming events");
        }
    }

    [HttpPost("{id}/participants")]
    public async Task<IActionResult> AddParticipant(string id, [FromBody] AddParticipantDto addParticipantDto)
    {
        try
        {
            var result = await _eventService.AddParticipantAsync(id, addParticipantDto.UserId, addParticipantDto.Role);
            
            _logger.LogInformation("Participant {UserId} added to event {EventId}", addParticipantDto.UserId, id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error adding participant to event {EventId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding participant to event {EventId}", id);
            return StatusCode(500, "Internal server error when adding participant");
        }
    }

    [HttpDelete("{id}/participants/{userId}")]
    public async Task<IActionResult> RemoveParticipant(string id, long userId)
    {
        try
        {
            var result = await _eventService.RemoveParticipantAsync(id, userId);
            
            if (result)
            {
                _logger.LogInformation("Participant {UserId} removed from event {EventId}", userId, id);
                return Ok(result);
            }
            else
            {
                return NotFound("Participant not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing participant from event {EventId}", id);
            return StatusCode(500, "Internal server error when removing participant");
        }
    }

    [HttpPut("{id}/participants/{userId}/status")]
    public async Task<IActionResult> UpdateParticipantStatus(string id, long userId, [FromBody] UpdateParticipantStatusDto statusDto)
    {
        try
        {
            var result = await _eventService.UpdateParticipantStatusAsync(id, userId, statusDto.Status, statusDto.Comment);
            
            _logger.LogInformation("Participant {UserId} status updated to {Status} for event {EventId}", userId, statusDto.Status, id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating participant status for event {EventId}", id);
            return StatusCode(500, "Internal server error when updating participant status");
        }
    }

    [HttpPost("{consId}/attachments")]
    public async Task<IActionResult> AddAttachment(string consId, [FromBody] AddAttachmentDto addAttachmentDto)
    {
        try
        {
            var result = await _eventService.AddAttachmentAsync(consId, addAttachmentDto.AttachmentId, addAttachmentDto.Description);
            
            _logger.LogInformation("Attachment {AttachmentId} added to event {EventId}", addAttachmentDto.AttachmentId, consId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error adding attachment to event {EventId}", consId);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding attachment to event {EventId}", consId);
            return StatusCode(500, "Internal server error when adding attachment");
        }
    }

    [HttpDelete("{id}/attachments/{attachmentId}")]
    public async Task<IActionResult> RemoveAttachment(string consId, long attachmentId)
    {
        try
        {
            var result = await _eventService.RemoveAttachmentAsync(consId, attachmentId);
            
            if (result)
            {
                _logger.LogInformation("Attachment {AttachmentId} removed from event {EventId}", attachmentId, consId);
                return Ok(result);
            }
            else
            {
                return NotFound("Attachment not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing attachment from event {EventId}", consId);
            return StatusCode(500, "Internal server error when removing attachment");
        }
    }

    [HttpPut("{id}/recurrence")]
    public async Task<IActionResult> UpdateRecurrence(string consId, [FromBody] UpdateRecurrenceDto recurrenceDto)
    {
        try
        {
            var result = await _eventService.UpdateRecurrenceAsync(consId, recurrenceDto.RecurrenceType, recurrenceDto.Interval, recurrenceDto.EndDate);
            
            _logger.LogInformation("Recurrence updated for event {EventId}", consId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating recurrence for event {EventId}", consId);
            return StatusCode(500, "Internal server error when updating recurrence");
        }
    }

    [HttpGet("{id}/recurring")]
    public async Task<IActionResult> GetRecurringEvents(string consId)
    {
        try
        {
            var result = await _eventService.GetRecurringEventsAsync(consId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recurring events for event {EventId}", consId);
            return StatusCode(500, "Internal server error when getting recurring events");
        }
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelEvent(string consId, [FromBody] CancelEventDto cancelDto)
    {
        try
        {
            var result = await _eventService.CancelEventAsync(consId, cancelDto.Reason);
            
            _logger.LogInformation("Event {EventId} cancelled", consId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling event {EventId}", consId);
            return StatusCode(500, "Internal server error when cancelling event");
        }
    }

    [HttpPost("{id}/reschedule")]
    public async Task<IActionResult> RescheduleEvent(string consId, [FromBody] RescheduleEventDto rescheduleDto)
    {
        try
        {
            var result = await _eventService.RescheduleEventAsync(consId, rescheduleDto.NewBeginDateTime, rescheduleDto.NewEndDateTime);
            
            _logger.LogInformation("Event {EventId} rescheduled", consId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rescheduling event {EventId}", consId);
            return StatusCode(500, "Internal server error when rescheduling event");
        }
    }

    [HttpPost("{consId}/invitations")]
    public async Task<IActionResult> SendInvitations(string consId)
    {
        try
        {
            var result = await _eventService.SendInvitationsAsync(consId);
            
            _logger.LogInformation("Invitations sent for event {EventId}", consId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending invitations for event {EventId}", consId);
            return StatusCode(500, "Internal server error when sending invitations");
        }
    }

    [HttpPost("{consId}/reminders")]
    public async Task<IActionResult> SendReminders(string consId)
    {
        try
        {
            var result = await _eventService.SendRemindersAsync(consId);
            
            _logger.LogInformation("Reminders sent for event {EventId}", consId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending reminders for event {EventId}", consId);
            return StatusCode(500, "Internal server error when sending reminders");
        }
    }

    [HttpPost("{consId}/sync-google")]
    public async Task<IActionResult> SyncWithGoogleCalendar(string consId)
    {
        try
        {
            var result = await _eventService.SyncWithGoogleCalendarAsync(consId);
            
            _logger.LogInformation("Event {EventId} synced with Google Calendar", consId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing event {EventId} with Google Calendar", consId);
            return StatusCode(500, "Internal server error when syncing with Google Calendar");
        }
    }

    [HttpPost("from-google")]
    public async Task<IActionResult> CreateFromGoogleCalendar([FromBody] GoogleCalendarEventDto googleEvent)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _eventService.CreateFromGoogleCalendarAsync(googleEvent, userId);
            
            _logger.LogInformation("Event created from Google Calendar for user {UserId}", userId);
            return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event from Google Calendar");
            return StatusCode(500, "Internal server error when creating event from Google Calendar");
        }
    }

    // Export functionality
    [HttpGet("{id}/export/ical")]
    public async Task<IActionResult> ExportEventToICalendar(string consId)
    {
        try
        {
            var icalContent = await _eventService.ExportEventToICalendarAsync(consId);
            
            var fileName = $"event_{consId}_{DateTime.UtcNow:yyyyMMdd}.ics";
            
            _logger.LogInformation("Event {EventId} exported to iCalendar format", consId);
            return File(Encoding.UTF8.GetBytes(icalContent), "text/calendar", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting event {EventId} to iCalendar", consId);
            return StatusCode(500, "Internal server error when exporting event to iCalendar");
        }
    }

    [HttpGet("user/export/ical")]
    public async Task<IActionResult> ExportUserEventsToICalendar([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var icalContent = await _eventService.ExportUserEventsToICalendarAsync(userId, fromDate, toDate);
            
            var fileName = $"user_events_{userId}_{DateTime.UtcNow:yyyyMMdd}.ics";
            
            _logger.LogInformation("User {UserId} events exported to iCalendar format", userId);
            return File(Encoding.UTF8.GetBytes(icalContent), "text/calendar", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting user events to iCalendar");
            return StatusCode(500, "Internal server error when exporting user events to iCalendar");
        }
    }

    [HttpPost("export/ical")]
    public async Task<IActionResult> ExportEventsToICalendar([FromBody] ExportEventsDto exportDto)
    {
        try
        {
            var icalContent = await _eventService.ExportEventsToICalendarAsync(exportDto.EventIds);
            
            var fileName = $"events_{DateTime.UtcNow:yyyyMMdd}.ics";
            
            _logger.LogInformation("{EventCount} events exported to iCalendar format", exportDto.EventIds.Count());
            return File(Encoding.UTF8.GetBytes(icalContent), "text/calendar", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting events to iCalendar");
            return StatusCode(500, "Internal server error when exporting events to iCalendar");
        }
    }

    [HttpGet("user/export/google-url")]
    public async Task<IActionResult> GetGoogleCalendarImportUrl([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var importUrl = await _eventService.GetGoogleCalendarImportUrlAsync(userId, fromDate, toDate);
            
            _logger.LogInformation("Google Calendar import URL generated for user {UserId}", userId);
            return Ok(new { importUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Google Calendar import URL for user");
            return StatusCode(500, "Internal server error when generating Google Calendar import URL");
        }
    }

    [HttpPost("export/google-url")]
    public async Task<IActionResult> GetGoogleCalendarImportUrlForEvents([FromBody] ExportEventsDto exportDto)
    {
        try
        {
            var importUrl = await _eventService.GetGoogleCalendarImportUrlAsync(exportDto.EventIds);
            
            _logger.LogInformation("Google Calendar import URL generated for {EventCount} events", exportDto.EventIds.Count());
            return Ok(new { importUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Google Calendar import URL for events");
            return StatusCode(500, "Internal server error when generating Google Calendar import URL");
        }
    }

    [HttpPost("{id}/export/google")]
    public async Task<IActionResult> ExportEventToGoogleCalendar(string consId, [FromBody] ExportToGoogleCalendarDto exportDto)
    {
        try
        {
            var success = await _eventService.ExportEventToGoogleCalendarAsync(consId, exportDto.UserAccessToken);
            
            if (success)
            {
                _logger.LogInformation("Event {EventId} exported to user's Google Calendar", consId);
                return Ok(new { success = true, message = "Event successfully exported to Google Calendar" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to export event to Google Calendar" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting event {EventId} to Google Calendar", consId);
            return StatusCode(500, "Internal server error when exporting event to Google Calendar");
        }
    }

    [HttpPost("user/export/google")]
    public async Task<IActionResult> ExportUserEventsToGoogleCalendar([FromBody] ExportUserEventsToGoogleDto exportDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _eventService.ExportUserEventsToGoogleCalendarAsync(userId, exportDto.UserAccessToken, exportDto.FromDate, exportDto.ToDate);
            
            if (success)
            {
                _logger.LogInformation("User {UserId} events exported to Google Calendar", userId);
                return Ok(new { success = true, message = "Events successfully exported to Google Calendar" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to export events to Google Calendar" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting user events to Google Calendar");
            return StatusCode(500, "Internal server error when exporting user events to Google Calendar");
        }
    }

    private string GetCurrentUserId()
    {
        /*var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID");
        }

        return userId;*/

        return "";
    }
}

// DTO classes for specific operations
public class AddParticipantDto
{
    public long UserId { get; set; }
    public ParticipantRole Role { get; set; } = ParticipantRole.Attendee;
}

public class UpdateParticipantStatusDto
{
    public ParticipantStatus Status { get; set; }
    public string? Comment { get; set; }
}
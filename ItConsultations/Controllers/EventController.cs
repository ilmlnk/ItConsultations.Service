using ItConsultations.Business.Dtos.AttachmentDtos;
using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Dtos.EventDtos;
using ItConsultations.Business.Services.EventService;
using ItConsultations.Business.SharedTypes.Enums.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ItConsultations.Controllers;

[Route("api/events")]
[Authorize]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createDto)
    {
        var userId = GetCurrentUserId();
        var result = await _eventService.CreateAsync(createDto, userId);

        //return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(long id, [FromBody] UpdateEventDto updateDto)
    {
        //updateDto.Id = id;
        var result = await _eventService.UpdateAsync(updateDto);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(long id)
    {
        var result = await _eventService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(long id)
    {
        var result = await _eventService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("cons/{consId}")]
    public async Task<IActionResult> GetEventByConsId(string consId)
    {
        var result = await _eventService.GetByConsIdAsync(consId);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchEvents([FromQuery] EventSearchDto searchDto)
    {
        var result = await _eventService.SearchAsync(searchDto);
        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserEvents([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var userId = GetCurrentUserId();
        //var result = await _eventService.GetUserEventsAsync(userId, fromDate, toDate);
        return Ok(null);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingEvents([FromQuery] int days = 7)
    {
        var result = await _eventService.GetUpcomingEventsAsync(days);
        return Ok(result);
    }

    [HttpPost("{id}/participants")]
    public async Task<IActionResult> AddParticipant(string id, [FromBody] AddParticipantDto addParticipantDto)
    {
        //var result = await _eventService.AddParticipantAsync(id, addParticipantDto.UserId, addParticipantDto);
        return Ok(null);
    }

    [HttpDelete("{id}/participants/{userId}")]
    public async Task<IActionResult> RemoveParticipant(string id, long userId)
    {
        var result = await _eventService.RemoveParticipantAsync(id, userId);
        return Ok(result);
    }

    [HttpPut("{id}/participants/{userId}/status")]
    public async Task<IActionResult> UpdateParticipantStatus(string id, long userId, [FromBody] UpdateParticipantStatusDto statusDto)
    {
        //var result = await _eventService.UpdateParticipantStatusAsync(id, userId, statusDto.Status, statusDto.Comment);
        return Ok(null);
    }

    [HttpPost("{consId}/attachments")]
    public async Task<IActionResult> AddAttachment(string consId, [FromBody] AddAttachmentDto addAttachmentDto)
    {
        //var result = await _eventService.AddAttachmentAsync(consId, addAttachmentDto.AttachmentId, addAttachmentDto.Description);
        return Ok(null);
    }

    [HttpDelete("{id}/attachments/{attachmentId}")]
    public async Task<IActionResult> RemoveAttachment(string consId, long attachmentId)
    {
        var result = await _eventService.RemoveAttachmentAsync(consId, attachmentId);
        return Ok(result);
    }

    [HttpPut("{id}/recurrence")]
    public async Task<IActionResult> UpdateRecurrence(string consId, [FromBody] UpdateRecurrenceDto recurrenceDto)
    {
        //var result = await _eventService.UpdateRecurrenceAsync(consId, recurrenceDto.RecurrenceType, recurrenceDto.Interval, recurrenceDto.EndDate);
        return Ok(null);
    }

    [HttpGet("{id}/recurring")]
    public async Task<IActionResult> GetRecurringEvents(string consId)
    {
        var result = await _eventService.GetRecurringEventsAsync(consId);
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelEvent(string consId, [FromBody] CancelEventDto cancelDto)
    {
        //var result = await _eventService.CancelEventAsync(consId, cancelDto.Reason);
        return Ok(null);
    }

    [HttpPost("{id}/reschedule")]
    public async Task<IActionResult> RescheduleEvent(string consId, [FromBody] RescheduleEventDto rescheduleDto)
    {
        //var result = await _eventService.RescheduleEventAsync(consId, rescheduleDto.NewBeginDateTime, rescheduleDto.NewEndDateTime);
        return Ok(null);
    }

    [HttpPost("{consId}/invitations")]
    public async Task<IActionResult> SendInvitations(string consId)
    {
        var result = await _eventService.SendInvitationsAsync(consId);
        return Ok(result);
    }

    [HttpPost("{consId}/reminders")]
    public async Task<IActionResult> SendReminders(string consId)
    {
        var result = await _eventService.SendRemindersAsync(consId);
        return Ok(result);
    }

    [HttpPost("{consId}/sync-google")]
    public async Task<IActionResult> SyncWithGoogleCalendar(string consId)
    {
        var result = await _eventService.SyncWithGoogleCalendarAsync(consId);
        return Ok(result);
    }

    [HttpPost("from-google")]
    public async Task<IActionResult> CreateFromGoogleCalendar([FromBody] GoogleCalendarEventDto googleEvent)
    {
        var userId = GetCurrentUserId();
        var result = await _eventService.CreateFromGoogleCalendarAsync(googleEvent, userId);
        //return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
        return Ok(result);
    }

    // Export functionality
    [HttpGet("{id}/export/ical")]
    public async Task<IActionResult> ExportEventToICalendar(string consId)
    {
        var icalContent = await _eventService.ExportEventToICalendarAsync(consId);
        var fileName = $"event_{consId}_{DateTime.UtcNow:yyyyMMdd}.ics";
        return File(Encoding.UTF8.GetBytes(icalContent), "text/calendar", fileName);
    }

    [HttpGet("user/export/ical")]
    public async Task<IActionResult> ExportUserEventsToICalendar([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var userId = GetCurrentUserId();
        //var icalContent = await _eventService.ExportUserEventsToICalendarAsync(userId, fromDate, toDate);

        var fileName = $"user_events_{userId}_{DateTime.UtcNow:yyyyMMdd}.ics";
        return File(Encoding.UTF8.GetBytes(new char[] { }), "text/calendar", fileName);
    }

    [HttpPost("export/ical")]
    public async Task<IActionResult> ExportEventsToICalendar([FromBody] ExportEventsDto exportDto)
    {
        var icalContent = await _eventService.ExportEventsToICalendarAsync(exportDto.EventIds);
        var fileName = $"events_{DateTime.UtcNow:yyyyMMdd}.ics";
        return File(Encoding.UTF8.GetBytes(icalContent), "text/calendar", fileName);
    }

    [HttpGet("user/export/google-url")]
    public async Task<IActionResult> GetGoogleCalendarImportUrl([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var userId = GetCurrentUserId();
        //var importUrl = await _eventService.GetGoogleCalendarImportUrlAsync(userId, fromDate, toDate);

        return Ok(null);
    }

    [HttpPost("export/google-url")]
    public async Task<IActionResult> GetGoogleCalendarImportUrlForEvents([FromBody] ExportEventsDto exportDto)
    {
        var importUrl = await _eventService.GetGoogleCalendarImportUrlAsync(exportDto.EventIds);
        return Ok(new { importUrl });
    }

    [HttpPost("{id}/export/google")]
    public async Task<IActionResult> ExportEventToGoogleCalendar(string consId, [FromBody] ExportToGoogleCalendarDto exportDto)
    {
        var success = await _eventService.ExportEventToGoogleCalendarAsync(consId, exportDto.UserAccessToken);
        return Ok(new { success = true, message = "Event successfully exported to Google Calendar" });
    }

    [HttpPost("user/export/google")]
    public async Task<IActionResult> ExportUserEventsToGoogleCalendar([FromBody] ExportUserEventsToGoogleDto exportDto)
    {
        var userId = GetCurrentUserId();
        //var success = await _eventService.ExportUserEventsToGoogleCalendarAsync(userId, exportDto.UserAccessToken, exportDto.FromDate, exportDto.ToDate);
        return Ok(new { success = true, message = "Events successfully exported to Google Calendar" });
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
/*public class AddParticipantDto
{
    public long UserId { get; set; }
    public ParticipantRole Role { get; set; } = ParticipantRole.Attendee;
}*/

public class UpdateParticipantStatusDto
{
    public ParticipantStatus Status { get; set; }
    public string? Comment { get; set; }
}
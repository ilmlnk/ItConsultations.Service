using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Dtos.EventDtos;

public class EventDto
{
    public long Id { get; set; }

    public string EventConsId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? MeetingUrl { get; set; }

    public string? MeetingProvider { get; set; }

    public List<string> AssigneeEmails { get; set; } = new();

    public List<EventParticipantDto> Participants { get; set; } = new();

    public UserEntity Creator { get; set; } = null!;

    public DateTime BeginDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public DateTime? ReminderTime { get; set; }

    public int? ReminderMinutes { get; set; }

    public RecurrenceType RecurrenceType { get; set; }

    public int? RecurrenceInterval { get; set; }

    public DayOfWeek? RecurrenceDayOfWeek { get; set; }

    public int? RecurrenceDayOfMonth { get; set; }

    public DateTime? RecurrenceEndDate { get; set; }

    public int? RecurrenceCount { get; set; }

    public EventStatus Status { get; set; }

    public EventVisibility Visibility { get; set; }

    public bool IsAllDay { get; set; }

    public string? GoogleCalendarEventId { get; set; }

    public string? GoogleCalendarId { get; set; }

    public DateTime? LastGoogleSync { get; set; }

    public string? Color { get; set; }

    public List<EventAttachmentDto> Attachments { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}

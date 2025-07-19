using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Dtos.EventDtos;

public class UpdateEventDto
{
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? MeetingUrl { get; set; }

    public string? MeetingProvider { get; set; }

    public List<string> AssigneeEmails { get; set; } = new();

    public List<long> ParticipantUserIds { get; set; } = new();

    public DateTime BeginDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public DateTime? ReminderTime { get; set; }

    public int? ReminderMinutes { get; set; } = 15;

    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;

    public int? RecurrenceInterval { get; set; } = 1;

    public DayOfWeek? RecurrenceDayOfWeek { get; set; }

    public int? RecurrenceDayOfMonth { get; set; }

    public DateTime? RecurrenceEndDate { get; set; }

    public int? RecurrenceCount { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Scheduled;

    public EventVisibility Visibility { get; set; } = EventVisibility.Private;

    public bool IsAllDay { get; set; } = false;

    public string? Color { get; set; }

    public List<long> AttachmentIds { get; set; } = new();
}

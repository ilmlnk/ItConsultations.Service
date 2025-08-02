using ItConsultations.Business.Dtos.GoogleCalendarDtos;

namespace ItConsultations.Business.Dtos.EventDtos;

public class GoogleCalendarEventDto
{
    public string? Id { get; set; }

    public string Summary { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public bool IsAllDay { get; set; } = false;

    public string? ColorId { get; set; }

    public GoogleCalendarAttendeeDto[]? Attendees { get; set; }

    public GoogleCalendarReminderDto[]? Reminders { get; set; }

    public GoogleCalendarRecurrenceDto[]? Recurrence { get; set; }

    public string? HangoutLink { get; set; }

    public string? ConferenceData { get; set; }
}


public class GoogleCalendarReminderDto
{
    public string Method { get; set; } = "email";

    public int Minutes { get; set; } = 15;
}

public class GoogleCalendarRecurrenceDto
{
    public string RRule { get; set; } = string.Empty;
} 
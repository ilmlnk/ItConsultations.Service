using ItConsultations.Business.Entities.Conferences;
using ItConsultations.Business.Entities.Events;
using ItConsultations.Business.Entities.Locations;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Dtos.EventDtos;

public class CreateEventDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Location? Location { get; set; }
    
    public Conference? Conference { get; set; }

    public List<EventParticipant> Participants { get; set; }

    public UserEntity Creator { get; set; }

    public DateTime StartDateTime { get; set; }

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

    public bool IsAllDay { get; set; } = false;

    public string? Color { get; set; }

    public List<long> AttachmentIds { get; set; } = new();
}

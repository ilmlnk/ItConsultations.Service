using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.User;
using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Entities.Event;

public class Event : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string EventConsId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? Location { get; set; }

    [MaxLength(500)]
    public string? MeetingUrl { get; set; }

    [MaxLength(100)]
    public string? MeetingProvider { get; set; } // Zoom, Teams, Google Meet, etc.

    public List<string> AssigneeEmails { get; set; } = new();

    public List<EventParticipant> Participants { get; set; } = new();

    [Required]
    public UserEntity Creator { get; set; } = null!;

    [Required]
    public DateTime BeginDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    public DateTime? ReminderTime { get; set; }

    public int? ReminderMinutes { get; set; } = 15; // Default 15 minutes before

    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;

    public int? RecurrenceInterval { get; set; } = 1;

    public DayOfWeek? RecurrenceDayOfWeek { get; set; }

    public int? RecurrenceDayOfMonth { get; set; }

    public DateTime? RecurrenceEndDate { get; set; }

    public int? RecurrenceCount { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Scheduled;

    public EventVisibility Visibility { get; set; } = EventVisibility.Private;

    public bool IsAllDay { get; set; } = false;

    public string? GoogleCalendarEventId { get; set; }

    public string? GoogleCalendarId { get; set; }

    public DateTime? LastGoogleSync { get; set; }

    public string? Color { get; set; } // Calendar color

    public List<EventAttachment> Attachments { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Entities.Events;

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
    public string? MeetingProvider { get; set; }

    public List<string> AssigneeEmails { get; set; } = new();

    public List<EventParticipant> Participants { get; set; } = new();

    [Required]
    public UserEntity Creator { get; set; } = null!;

    [Required]
    public DateTime BeginDateTime { get; set; }

    [Required]
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

    public string? GoogleCalendarEventId { get; set; }

    public string? GoogleCalendarId { get; set; }

    public DateTime? LastGoogleSync { get; set; }

    public string? Color { get; set; }

    public List<EventAttachment> Attachments { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}

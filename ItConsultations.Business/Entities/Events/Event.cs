using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Conferences;
using ItConsultations.Business.Entities.Locations;
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
    public string EventConsId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }
    
    public Conference? Conference { get; set; }
    
    public Location Location { get; set; }

    public List<EventParticipant> Participants { get; set; }

    [Required]
    public UserEntity Creator { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }

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

    public bool IsAllDay { get; set; }

    public string? GoogleCalendarEventId { get; set; }

    public string? GoogleCalendarId { get; set; }

    public DateTime? LastGoogleSync { get; set; }

    public string? Color { get; set; }

    public List<EventAttachment> Attachments { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}

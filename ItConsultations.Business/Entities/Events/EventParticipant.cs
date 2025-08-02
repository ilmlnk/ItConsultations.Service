using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Entities.Events;

public class EventParticipant : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string ParticipantConsId { get; set; } = string.Empty;

    [Required]
    public string EventConsId { get; set; }

    [Required]
    public Event Event { get; set; } = null!;

    [Required]
    public long UserId { get; set; }

    [Required]
    public UserEntity User { get; set; } = null!;

    public ParticipantRole Role { get; set; } = ParticipantRole.Attendee;

    public ParticipantStatus Status { get; set; } = ParticipantStatus.Pending;

    public DateTime? ResponseDate { get; set; }

    public string? ResponseComment { get; set; }

    public bool IsRequired { get; set; } = false;

    public bool SendReminders { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Dtos.EventDtos;

public class EventParticipantDto
{
    public long Id { get; set; }

    public string ParticipantConsId { get; set; } = string.Empty;

    public string EventConsId { get; set; }

    public long UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public ParticipantRole Role { get; set; }

    public ParticipantStatus Status { get; set; }

    public DateTime? ResponseDate { get; set; }

    public string? ResponseComment { get; set; }

    public bool IsRequired { get; set; }

    public bool SendReminders { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
} 
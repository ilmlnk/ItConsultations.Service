using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Dtos.ConferenceDtos.Conference;

public class ConferenceParticipantDto
{
    public long Id { get; set; }

    public string ParticipantConsId { get; set; } = string.Empty;

    public string ConferenceConsId { get; set; } = string.Empty;

    public long UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public ParticipantRole Role { get; set; }

    public ParticipantStatus Status { get; set; }

    public DateTime? JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
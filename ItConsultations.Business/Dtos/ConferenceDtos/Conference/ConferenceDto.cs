using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Dtos.ConferenceDtos.Conference;

public class ConferenceDto
{
    public long Id { get; set; }

    public string ConferenceConsId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? MeetingUrl { get; set; }

    public string? MeetingProvider { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public ConferenceStatus Status { get; set; }

    public ConferenceType Type { get; set; }

    public int MaxParticipants { get; set; }

    public bool IsRecordingEnabled { get; set; }

    public bool IsChatRecordingEnabled { get; set; }

    public string? Password { get; set; }

    public bool IsPasswordProtected { get; set; }

    public UserEntity Creator { get; set; } = null!;

    public List<ConferenceParticipantDto> Participants { get; set; } = new();

    public List<ConferenceRecordingDto> Recordings { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
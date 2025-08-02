using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Dtos.ConferenceDtos.Conference;

public class UpdateConferenceDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? MeetingUrl { get; set; }

    public string? MeetingProvider { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public ConferenceType Type { get; set; }

    public int MaxParticipants { get; set; }

    public bool IsRecordingEnabled { get; set; }

    public bool IsChatRecordingEnabled { get; set; }

    public string? Password { get; set; }

    public bool IsPasswordProtected { get; set; }

    public List<long> ParticipantUserIds { get; set; } = new();
}
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Dtos.ConferenceDtos.Conference;

public class CreateConferenceDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? MeetingUrl { get; set; }

    public string? MeetingProvider { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public ConferenceType Type { get; set; } = ConferenceType.Consultation;

    public int MaxParticipants { get; set; } = 10;

    public bool IsRecordingEnabled { get; set; } = false;

    public bool IsChatRecordingEnabled { get; set; } = false;

    public string? Password { get; set; }

    public bool IsPasswordProtected { get; set; } = false;

    public List<long> ParticipantUserIds { get; set; } = new();
}
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Dtos.ConferenceDtos.Conference;

public class ConferenceRecordingDto
{
    public long Id { get; set; }

    public string RecordingConsId { get; set; } = string.Empty;

    public string ConferenceConsId { get; set; } = string.Empty;

    public RecordingType Type { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string ContentType { get; set; } = string.Empty;

    public DateTime RecordedAt { get; set; }

    public TimeSpan Duration { get; set; }

    public RecordingStatus Status { get; set; }

    public string? ThumbnailUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Dtos.ConferenceDtos.Conference;

public class ConferenceSearchDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? StartTimeFrom { get; set; }

    public DateTime? StartTimeTo { get; set; }

    public long? CreatorId { get; set; }

    public ConferenceStatus? Status { get; set; }

    public ConferenceType? Type { get; set; }

    public bool? IsRecordingEnabled { get; set; }

    public bool? IsPasswordProtected { get; set; }

    public string? MeetingProvider { get; set; }

    public int PageSize { get; set; } = 20;

    public int PageNumber { get; set; } = 1;

    public string? SortBy { get; set; }

    public string SortDirection { get; set; } = "asc";
}
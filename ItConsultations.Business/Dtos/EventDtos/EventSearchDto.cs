using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Dtos.EventDtos;

public class EventSearchDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? BeginDateFrom { get; set; }

    public DateTime? BeginDateTo { get; set; }

    public DateTime? EndDateFrom { get; set; }

    public DateTime? EndDateTo { get; set; }

    public long? CreatorId { get; set; }

    public long? ParticipantUserId { get; set; }

    public EventStatus? Status { get; set; }

    public EventVisibility? Visibility { get; set; }

    public RecurrenceType? RecurrenceType { get; set; }

    public bool? IsAllDay { get; set; }

    public string? Location { get; set; }

    public bool? HasMeetingUrl { get; set; }

    public string? MeetingProvider { get; set; }

    public bool? HasAttachments { get; set; }

    public int PageSize { get; set; } = 20;

    public int PageNumber { get; set; } = 1;

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; } = "asc";
} 
using ItConsultations.Business.SharedTypes.Enums.Consultation;

namespace ItConsultations.Business.Dtos.NoteDtos;

public class NoteSearchDto
{
    public string? SearchText { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public NoteType? Type { get; set; }

    public NoteVisibility? Visibility { get; set; }

    public NotePriority? Priority { get; set; }

    public NoteStatus? Status { get; set; }

    public List<string> Tags { get; set; } = new();

    public long? ConsultationId { get; set; }

    public long? CoachId { get; set; }

    public long? StudentId { get; set; }

    public long? AuthorId { get; set; }

    public DateTime? CreatedFrom { get; set; }

    public DateTime? CreatedTo { get; set; }

    public DateTime? UpdatedFrom { get; set; }

    public DateTime? UpdatedTo { get; set; }

    public DateTime? ScheduledFrom { get; set; }

    public DateTime? ScheduledTo { get; set; }

    public string? Location { get; set; }

    public string? Source { get; set; }

    public bool? IsPinned { get; set; }

    public bool? IncludeDeleted { get; set; } = false;

    public string? SortBy { get; set; } = "CreatedAt";

    public string? SortDirection { get; set; } = "desc";
}
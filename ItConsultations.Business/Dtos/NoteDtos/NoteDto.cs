using ItConsultations.Business.SharedTypes.Enums.Consultation;

namespace ItConsultations.Business.Dtos.NoteDtos;

public class NoteDto
{
    public long Id { get; set; }

    public string NoteConsId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public NoteType Type { get; set; }

    public NoteVisibility Visibility { get; set; }

    public NotePriority Priority { get; set; }

    public NoteStatus Status { get; set; }

    public long? ConsultationId { get; set; }

    public long? CoachId { get; set; }

    public long? StudentId { get; set; }

    public long? ConferenceId { get; set; }

    public long AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public string AuthorEmail { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new();

    public string? Location { get; set; }

    public string? Source { get; set; }

    public DateTime? ScheduledFor { get; set; }

    public bool IsPinned { get; set; }

    public int ViewCount { get; set; }

    public DateTime? LastViewedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
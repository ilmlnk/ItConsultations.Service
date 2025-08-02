using ItConsultations.Business.SharedTypes.Enums.Consultation;

namespace ItConsultations.Business.Dtos.NoteDtos;

public class CreateNoteDto
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public NoteType Type { get; set; } = NoteType.General;

    public NoteVisibility Visibility { get; set; } = NoteVisibility.Private;

    public NotePriority Priority { get; set; } = NotePriority.Normal;

    public long? ConsultationId { get; set; }

    public long? CoachId { get; set; }

    public long? StudentId { get; set; }

    public List<string> Tags { get; set; } = new();

    public string? Location { get; set; }

    public string? Source { get; set; }

    public DateTime? ScheduledFor { get; set; }

    public bool IsPinned { get; set; } = false;
}
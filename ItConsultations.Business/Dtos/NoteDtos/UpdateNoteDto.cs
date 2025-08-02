using ItConsultations.Business.SharedTypes.Enums.Consultation;

namespace ItConsultations.Business.Dtos.NoteDtos;

public class UpdateNoteDto
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public NoteType Type { get; set; }

    public NoteVisibility Visibility { get; set; }

    public NotePriority Priority { get; set; }

    public NoteStatus Status { get; set; }

    public List<string> Tags { get; set; } = new();

    public string? Location { get; set; }

    public string? Source { get; set; }

    public DateTime? ScheduledFor { get; set; }

    public bool IsPinned { get; set; }
}
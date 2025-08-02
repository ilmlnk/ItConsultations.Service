using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Consultation;
using ItConsultations.Business.Entities.Coaches;
using ItConsultations.Business.Entities.Students;

namespace ItConsultations.Business.Entities.Notes;

public class Note : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string NoteConsId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public NoteType Type { get; set; }

    [Required]
    public NoteVisibility Visibility { get; set; }

    public long? ConsultationId { get; set; }

    public Consultations.Consultation? Consultation { get; set; }

    public long? CoachId { get; set; }

    public Coach? Coach { get; set; }

    public long? StudentId { get; set; }

    public Student? Student { get; set; }

    public long? ConferenceId { get; set; }

    public string? ConferenceConsId { get; set; }

    [Required]
    public long AuthorId { get; set; }

    public string UserConsId { get; set; }

    public UserEntity Author { get; set; }

    public List<string> Tags { get; set; } = new();

    public NotePriority Priority { get; set; } = NotePriority.Normal;

    public NoteStatus Status { get; set; } = NoteStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    public string? Location { get; set; }

    public string? Source { get; set; }

    public DateTime? ScheduledFor { get; set; }

    public bool IsPinned { get; set; } = false;

    public int ViewCount { get; set; } = 0;

    public DateTime? LastViewedAt { get; set; }
}
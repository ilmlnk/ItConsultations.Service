using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Notes;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Entities.Conferences;

public class Conference : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string ConferenceConsId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Required]
    public long OrganizerId { get; set; }

    public UserEntity Organizer { get; set; }

    public long? ConsultationId { get; set; }

    public string ConsultationConsId { get; set; }

    public Consultations.Consultation? Consultation { get; set; }

    [Required]
    [MaxLength(300)]
    public string ConferenceUrl { get; set; } = string.Empty;

    public ConferenceStatus Status { get; set; } = ConferenceStatus.Scheduled;

    public bool IsRecordingEnabled { get; set; } = false;

    public bool IsChatRecordingEnabled { get; set; } = false;

    public List<ConferenceParticipant> Participants { get; set; } = new();

    public List<Note> Notes { get; set; } = new();

    public List<ConferenceRecording> Recordings { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

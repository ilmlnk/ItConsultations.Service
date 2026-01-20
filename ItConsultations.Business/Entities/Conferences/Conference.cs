using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Consultations;
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
    public string Title { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }
    
    [MaxLength(500)]
    public string? Location { get; set; }
    
    [MaxLength(100)]
    public string? MeetingProvider { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }
    
    public List<ConferenceParticipant> Participants { get; set; }

    public UserEntity Organizer { get; set; }
    
    public long OrganizerId { get; set; }

    public Consultation? Consultation { get; set; }
    
    public string? ConsultationConsId { get; set; }

    [Required]
    [MaxLength(300)]
    public string ConferenceUrl { get; set; }

    public ConferenceStatus Status { get; set; }

    public bool IsRecordingEnabled { get; set; }

    public bool IsChatRecordingEnabled { get; set; }

    public List<Note> Notes { get; set; }

    public List<ConferenceRecording> Recordings { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

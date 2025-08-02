using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Conferences;

public class ConferenceRecording : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string ConferenceRecordingConsId { get; set; }

    public Conference Conference { get; set; }

    public string? RecordingUrl { get; set; }

    public string? ChatLogUrl { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public bool IsActive { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

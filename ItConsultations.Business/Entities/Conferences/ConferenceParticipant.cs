using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Entities.Conferences;

public class ConferenceParticipant : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string ConferenceConsId { get; set; }

    public Conference Conference { get; set; }

    [Required]
    public long UserId { get; set; }

    public UserEntity User { get; set; }

    public ConferenceParticipantRole Role { get; set; }

    public DateTime? JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }
}

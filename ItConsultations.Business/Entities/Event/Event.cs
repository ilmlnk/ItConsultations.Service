using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Event;

public class Event : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(32)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string EventConsId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<string> AssigneeEmails { get; set; }

    [Required]
    public User.User Creator { get; set; }

    [Required]
    public DateTime BeginDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

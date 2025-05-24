using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.Event;

public class Event : Entity<long>
{
    [Required]
    [MaxLength(32)]
    public Guid Id { get; set; }

    [MaxLength(32)]
    public string EventConsId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<string> AssigneeEmails { get; set; }

    [Required]
    public Entity<long> Creator { get; set; }

    [Required]
    public DateTime BeginDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

using ItConsultations.Business.Entities;
using ItConsultations.Business.Entities.User;

namespace ItConsultations.Business.Dtos.EventDtos;

public class UpdateEventDto
{
    public long Id { get; set; }

    public string EventConsId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<string> AssigneeEmails { get; set; }

    public User Creator { get; set; }

    public DateTime BeginDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Users;

namespace ItConsultations.Business.Entities.Events;

[Table("ViewEventList")]
public class ViewEventList
{
    public long Id { get; set; }

    public string EventConsId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<string> AssigneeEmails { get; set; }

    public UserEntity Creator { get; set; }

    public DateTime BeginDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

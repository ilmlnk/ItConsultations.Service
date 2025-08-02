using ItConsultations.Business.Entities.Consultations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Coaches;

[Table("ViewCoachList")]
public class ViewCoachList
{
    public long Id { get; set; }

    public string CoachConsId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string LinkedInUrl { get; set; }

    public string GitHubUrl { get; set; }

    public List<Consultation> Consultations { get; set; }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Consultation;

[Table("ViewStudentList")]
public class ViewStudentList
{
    public long Id { get; set; }

    public string StudentConsId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string GitHubUrl { get; set; }

    public string LinkedInUrl { get; set; }

    public Consultation Consultation { get; set; }
}

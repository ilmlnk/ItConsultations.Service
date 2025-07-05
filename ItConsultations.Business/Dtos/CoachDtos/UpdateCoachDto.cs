using ItConsultations.Business.Entities.Consultation;

namespace ItConsultations.Business.Dtos.CoachDtos;

public class UpdateCoachDto
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; }

    public string LinkedInUrl { get; set; }

    public string GitHubUrl { get; set; }
}

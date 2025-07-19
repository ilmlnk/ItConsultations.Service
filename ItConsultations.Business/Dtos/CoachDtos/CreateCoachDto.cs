namespace ItConsultations.Business.Dtos.CoachDtos;

public class CreateCoachDto
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Description { get; set; }

    public string Email { get; set; }

    public string LinkedInUrl { get; set; }

    public string GitHubUrl { get; set; }
}

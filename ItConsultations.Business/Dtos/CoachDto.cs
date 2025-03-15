namespace ItConsultations.Business.Dtos;

public class CoachDto
{
    public long Id { get; set; }

    public string CoachConsId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }
}

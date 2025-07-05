namespace ItConsultations.Business.Dtos.StudentDtos;

public class CreateStudentDto
{
    public long Id { get; set; }

    public string ConsStudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string LinkedInUrl { get; set; }

    public string GitHubUrl { get; set; }
}

namespace ItConsultations.Business.Dtos.StudentDtos;

public class CreateStudentDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }
}

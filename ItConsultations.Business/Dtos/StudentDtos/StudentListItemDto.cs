namespace ItConsultations.Business.Dtos.StudentDtos;

public class StudentListItemDto
{
    public long Id { get; set; }

    public string StudentConsId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }
}

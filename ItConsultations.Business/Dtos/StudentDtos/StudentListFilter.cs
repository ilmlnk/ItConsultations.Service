using ItConsultations.Business.Dtos.Shared;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Dtos.StudentDtos;

public class StudentListFilter : PagedFilter
{
    [Required]
    public long Id { get; set; }

    public string StudentConsId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; } 

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }
}

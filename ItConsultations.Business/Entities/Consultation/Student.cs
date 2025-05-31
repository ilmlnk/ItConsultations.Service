using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.Consultation;

public class Student : Entity<long>
{
    [Required]
    [MaxLength(32)]
    public long Id { get; set; }

    [MaxLength(32)]
    public string StudentConsId { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string? GitHubUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public Consultation Consultation { get; set; }
}

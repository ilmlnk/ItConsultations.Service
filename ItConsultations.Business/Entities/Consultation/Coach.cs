using System.ComponentModel.DataAnnotations;
namespace ItConsultations.Business.Entities.Consultation;

public class Coach : Entity<long>
{
    [Required]
    [MaxLength(32)]
    public Guid Id { get; set; }

    [MaxLength(32)]
    public string CoachConsId { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string Description { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public List<Consultation> Consultations { get; set; }

    public List<Review> Reviews { get; set; }

    public decimal AverageRating { get; set; }
}

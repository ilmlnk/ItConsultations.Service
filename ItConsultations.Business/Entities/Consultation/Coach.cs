using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.User;

namespace ItConsultations.Business.Entities.Consultation;

public class Coach : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(32)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string CoachConsId { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Description { get; set; }

    [Required]
    public string Email { get; set; }

    public string? PictureUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public List<Consultation> Consultations { get; set; }

    public List<Review> Reviews { get; set; }

    public decimal AverageRating { get; set; }

    public UserEntity User { get; set; } = null!;
}

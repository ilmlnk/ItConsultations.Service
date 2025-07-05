using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Consultation;

public class Student : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(32)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string StudentConsId { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string PictureUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public Consultation Consultation { get; set; }

    public ItConsultations.Business.Entities.User.User User { get; set; } = null!;
}

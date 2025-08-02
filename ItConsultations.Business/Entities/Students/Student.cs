using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ItConsultations.Business.Entities.Consultations;
using ItConsultations.Business.Entities.Users;

namespace ItConsultations.Business.Entities.Students;

public class Student : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    public string? PictureUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    [JsonIgnore]
    public Consultation? Consultation { get; set; }

    [JsonIgnore]
    public UserEntity User { get; set; } = null!;
}

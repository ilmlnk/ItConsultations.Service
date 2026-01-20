using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.Entities.Reviews;
using ItConsultations.Business.Entities.Consultations;
using ItConsultations.Business.Entities.RegionalSettings;
using ItConsultations.Business.SharedTypes.Enums.Coach;

namespace ItConsultations.Business.Entities.Coaches;

public class Coach : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string CoachConsId { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }
    
    [Required]
    public string CompanyName { get; set; }
    
    public string CompanyImageUrl { get; set; }

    public string Username { get; set; }

    public string Description { get; set; }

    [Required]
    public string Email { get; set; }
    
    [Required]
    public string CompanyPosition { get; set; }

    public string? PictureUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }
    
    public string? TelegramUrl { get; set; }
    
    public string? VideoCardUrl { get; set; }

    public CoachApplicationStatus CoachApplicationStatus { get; set; }

    public List<Consultation>? Consultations { get; set; }

    public List<Review>? Reviews { get; set; }

    [NotMapped]
    public decimal AverageRating { get; set; }
    
    public List<string> Topics { get; set; }
    
    public List<string> Skills { get; set; }
    
    public List<Language> Languages { get; set; }

    public UserEntity User { get; set; }
}

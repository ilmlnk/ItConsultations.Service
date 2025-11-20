using ItConsultations.Business.Entities.Coaches;
using ItConsultations.Business.Entities.Students;
using ItConsultations.Business.SharedTypes.Enums.Consultation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Consultations;

public class Consultation : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
    public string ConsId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<ConsultationCategoryType>? Categories { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsFavorite { get; set; } = false;

    [Required]
    public decimal Price { get; set; }

    public DateTime Duration { get; set; }

    public string ThumbnailUrl { get; set; }

    public Coach Coach { get; set; }

    public List<Student> Students { get; set; }
}

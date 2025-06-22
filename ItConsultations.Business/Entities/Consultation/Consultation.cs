using ItConsultations.Business.SharedTypes;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.Consultation;

public class Consultation : Entity<long>
{
    [Required]
    [MaxLength(32)]
    public long Id { get; set; }

    [MaxLength(32)]
    public string ConsId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<ConsultationCategoryType>? Categories { get; set; }

    [Required]
    public decimal Price { get; set; }

    public DateTime Duration { get; set; }

    public string ThumbnailUrl { get; set; }

    public Coach Coach { get; set; }

    public List<Student> Students { get; set; }
}

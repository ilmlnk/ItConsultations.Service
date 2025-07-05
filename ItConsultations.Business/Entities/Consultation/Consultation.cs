using ItConsultations.Business.SharedTypes.Enums.Consultation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Consultation;

public class Consultation : Entity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(32)]
    [Required]
    public long Id { get; set; }

    [MaxLength(36)]
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

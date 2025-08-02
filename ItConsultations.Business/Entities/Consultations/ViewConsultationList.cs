using ItConsultations.Business.Entities.Coaches;
using ItConsultations.Business.SharedTypes.Enums.Consultation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Consultations;

[Table("ViewConsultationList")]
public class ViewConsultationList
{
    public long Id { get; set; }

    public string ConsId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public ConsultationCategoryType[]? Categories { get; set; }

    public decimal Price { get; set; }

    public string ThumbnailUrl { get; set; }

    public Coach Coach { get; set; }
}

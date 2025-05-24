using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Business.SharedTypes;

namespace ItConsultations.Business.Dtos.ConsultationDtos;

public class ConsultationDto
{
    public long Id { get; set; }

    public string ConsId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public ConsultationCategoryType[]? Categories { get; set; }

    public decimal Price { get; set; }

    public DateTime Duration { get; set; }

    public string ThumbnailUrl { get; set; }

    public Coach Coach { get; set; }
}

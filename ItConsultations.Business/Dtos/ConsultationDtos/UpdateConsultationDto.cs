using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Business.SharedTypes;

namespace ItConsultations.Business.Dtos.ConsultationDtos;

public class UpdateConsultationDto
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public List<ConsultationCategoryType> Categories { get; set; }

    public decimal Price { get; set; }

    public DateTime Duration { get; set; }

    public string ThumbnailUrl { get; set; }

    public Coach Coach { get; set; }

    public List<Student> Students { get; set; }
}

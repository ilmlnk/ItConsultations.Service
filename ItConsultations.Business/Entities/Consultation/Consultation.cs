using ItConsultations.Business.SharedTypes;

namespace ItConsultations.Business.Entities.Consultation;


public class Consultation : Entity<long>
{
    public string Id {  get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ConsultationCategoryType[] Categories { get; set; }
    public decimal Price { get; set; }
    public string ThumbnailUrl { get; set; }
    public Coach Coach { get; set; }
}

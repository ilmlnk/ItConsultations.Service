using ItConsultations.Business.Entities.Attachments;

namespace ItConsultations.Business.Dtos.CoachDtos;

public class DeclineCoachDto
{
    public string CoachConsId { get; set; }
    
    public string Reason { get; set; }

    public List<Attachment>? Attachments { get; set; }
}

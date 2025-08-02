using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Dtos.EventDtos;

public class UpdateRecurrenceDto
{
    public RecurrenceType RecurrenceType { get; set; }

    public int? Interval { get; set; }

    public DateTime? EndDate { get; set; }
}

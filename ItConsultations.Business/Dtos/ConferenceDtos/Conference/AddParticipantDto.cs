using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Dtos.ConferenceDtos.Conference;

public class AddParticipantDto
{
    public long UserId { get; set; }
    public ParticipantRole Role { get; set; } = ParticipantRole.Participant;
}

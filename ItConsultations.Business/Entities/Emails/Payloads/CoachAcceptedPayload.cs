namespace ItConsultations.Business.Entities.Emails.Payloads;

public class CoachAcceptedPayload : BaseEmailPayload
{
    public string FirstName { get; set; }

    public string CoachConsId { get; set; }

    public string LoginUrl { get; set; }
}

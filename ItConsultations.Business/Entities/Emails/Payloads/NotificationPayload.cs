namespace ItConsultations.Business.Entities.Emails.Payloads;

public class NotificationPayload : BaseEmailPayload
{
    public List<string> Messages { get; set; }
}

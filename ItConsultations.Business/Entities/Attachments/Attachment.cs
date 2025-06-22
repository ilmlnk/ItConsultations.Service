namespace ItConsultations.Business.Entities.Attachments;

public class Attachment : AttachmentBase
{
    public long? EntityId { get; set; }

    public object EntityName { get; set; }

    public string Id { get; set; }
}

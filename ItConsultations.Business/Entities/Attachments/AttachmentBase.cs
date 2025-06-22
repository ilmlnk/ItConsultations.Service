namespace ItConsultations.Business.Entities.Attachments;

public class AttachmentBase : IEntity<long>
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string FileName { get; set; }

    public DateTime CreatedAt { get; set; }
}

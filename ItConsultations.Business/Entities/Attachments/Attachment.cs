using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItConsultations.Business.Entities.Attachments;

public class Attachment : AttachmentBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(32)]
    [Required]
    public long Id { get; set; }

    public long? EntityId { get; set; }

    public object EntityName { get; set; }

    public string AttachmentId { get; set; }
}

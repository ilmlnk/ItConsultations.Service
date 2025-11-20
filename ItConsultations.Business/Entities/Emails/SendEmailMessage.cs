using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Entities.Emails;

public class SendEmailMessage
{
    public string ToEmail { get; set; }

    public string Ccs { get; set; }

    public string Subject { get; set; }

    public string Language { get; set; }

    public EmailTemplateType EmailTemplateType { get; set; }

    public object Model { get; set; }

    public List<string>? AttachmentFilePaths { get; set; }
}

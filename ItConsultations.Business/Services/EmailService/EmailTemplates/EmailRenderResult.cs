using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Services.EmailService.EmailTemplates;

public class EmailRenderResult
{
    public EmailTemplateType TemplateType { get; set; }
    public string Subject { get; set; }
    public object Model { get; set; }
}

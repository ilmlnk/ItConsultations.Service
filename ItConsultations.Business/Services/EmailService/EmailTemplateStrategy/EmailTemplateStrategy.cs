using ItConsultations.Business.Entities.Emails.Payloads;
using ItConsultations.Business.Services.EmailService.EmailTemplates;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Services.EmailService.EmailTemplateStrategy;

public class EmailTemplateStrategy : IEmailTemplateStrategy
{
    public EmailTemplateType EmailTemplateType { get; }

    public EmailRenderResult PrepareData(BaseEmailPayload payload, string culture)
    {
        throw new NotImplementedException();
    }
}

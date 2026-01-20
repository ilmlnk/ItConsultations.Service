using ItConsultations.Business.Entities.Emails.Payloads;
using ItConsultations.Business.Services.EmailService.EmailTemplates;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Services.EmailService.EmailTemplateStrategy;

public interface IEmailTemplateStrategy
{
    EmailTemplateType EmailTemplateType { get; }
    EmailRenderResult PrepareData(BaseEmailPayload payload, string culture);
}

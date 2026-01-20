using ItConsultations.Business.Entities.Emails.Payloads;
using ItConsultations.Business.Services.EmailService.EmailTemplates;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Services.EmailService.EmailFactory;

public interface IEmailFactory
{
    EmailRenderResult Create(EmailTemplateType type, BaseEmailPayload payload, string culture);
}

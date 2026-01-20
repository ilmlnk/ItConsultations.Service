using System.Diagnostics;
using ItConsultations.Business.Entities.Emails.Payloads;
using ItConsultations.Business.Services.EmailService.EmailTemplates;
using ItConsultations.Business.Services.EmailService.EmailTemplateStrategy;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Services.EmailService.EmailFactory;

public class EmailFactory : IEmailFactory
{
    private readonly IEnumerable<IEmailTemplateStrategy> _strategies;

    public EmailFactory(IEnumerable<IEmailTemplateStrategy> strategies)
    {
        _strategies = strategies;
    }

    public EmailRenderResult Create(EmailTemplateType type, BaseEmailPayload payload, string culture)
    {
        var strategy = _strategies.FirstOrDefault(s => s.EmailTemplateType == type);
        return strategy.PrepareData(payload, culture);
    }
}

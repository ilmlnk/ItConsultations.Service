using ItConsultations.Business.Entities.Emails;
using ItConsultations.Business.Entities.Emails.Payloads;
using ItConsultations.Business.Services.EmailService.EmailFactory;
using ItConsultations.Business.Services.EmailService.EmailTemplates;
using ItConsultations.Business.Services.UnsubscribeService;
using ItConsultations.Business.SharedTypes.Enums.System;
using ItConsultations.Translation;
using MassTransit;

namespace ItConsultations.Business.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IEmailFactory _emailFactory;
    private readonly IUnsubscribeService _unsubscribeService;
    private readonly ITranslationService _translationService;
    
    public EmailService(
        IPublishEndpoint publishEndpoint, 
        IEmailFactory emailFactory,
        IUnsubscribeService unsubscribeService,
        ITranslationService translationService)
    {
        _publishEndpoint = publishEndpoint;
        _emailFactory = emailFactory;
        _unsubscribeService = unsubscribeService;
        _translationService = translationService;
    }

    public async Task SendCoachAcceptedAsync(string coachConsId, string to, string ccs, string culture)
    {
        long userId = 123;
        string firstName = "example";
        var unsubcribeToken = await _unsubscribeService.GenerateSecureToken(userId);
        var loginUrl = "";

        var payload = new CoachAcceptedPayload
        {
            FirstName = firstName,
            CoachConsId = coachConsId,
            LoginUrl = loginUrl,
            UnsubscribeToken = unsubcribeToken
        };

        var renderResult = _emailFactory.Create(EmailTemplateType.CoachAccepted, payload, culture);

        await PublishEmailMessageAsync(to, ccs, culture, renderResult, new List<string>());
    }

    public Task SendDecisionEmailAsync(string toEmail, string consultationId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailWithAttachmentsAsync(string to, string subject, EmailTemplateType emailTemplateType, List<EmailAttachment> attachments)
    {
        throw new NotImplementedException();
    }

    private async Task PublishEmailMessageAsync(
        string to,
        string ccs,
        string language,
        EmailRenderResult renderResult,
        List<string> paths)
    {
        string finalSubject = _translationService.GetTranslation(renderResult.Subject, language);

        await _publishEndpoint.Publish(new SendEmailMessage
        {
            ToEmail = to,
            Ccs = ccs,
            Subject = finalSubject,
            Language = language,
            EmailTemplateType = renderResult.EmailTemplateType,
            Model = renderResult.Model,
            AttachmentFilePaths = paths
        });
    }
}

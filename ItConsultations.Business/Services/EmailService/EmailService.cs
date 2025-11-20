using ItConsultations.Business.Entities.Emails;
using ItConsultations.Business.SharedTypes.Enums.System;
using MassTransit;

namespace ItConsultations.Business.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly IPublishEndpoint _publishEndpoint;
    public EmailService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendCoachAcceptedAsync(string coachConsId, string to, string ccs, string culture)
    {
        await _publishEndpoint.Publish(new SendEmailMessage
        {

        });
    }

    public Task SendDecisionEmailAsync(string toEmail, string consultationId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailWithAttachmentsAsync(string to, string subject, EmailTemplateType emailTemplateType, List<EmailAttachment> attachments)
    {
        throw new NotImplementedException();
    }
}

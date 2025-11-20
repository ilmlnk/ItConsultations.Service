using ItConsultations.Business.Entities.Emails;
using ItConsultations.Business.SharedTypes.Enums.System;

namespace ItConsultations.Business.Services.EmailService;

public interface IEmailService
{
    Task SendDecisionEmailAsync(string toEmail, string consultationId, bool isApproved);

    Task SendEmailWithAttachmentsAsync(string to, string subject, EmailTemplateType emailTemplateType, List<EmailAttachment> attachments);
}

using ItConsultations.Business.Entities.Emails;
using ItConsultations.Business.Services.EmailService;
using ItConsultations.Business.Services.EmailService.EmailTemplateService;
using MassTransit;

namespace ItConsultations.Infrastructure;

public class EmailConsumer : IConsumer<SendEmailMessage>
{
    private readonly IEmailService _emailService;
    private readonly FluidEmailTemplateService _templateService;

    public EmailConsumer(
        FluidEmailTemplateService templateService,
        IEmailService emailService
    )
    {
        _templateService = templateService;
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<SendEmailMessage> context)
    {
        var msg = context.Message;
        string htmlBody = await _templateService.GenerateEmailBodyAsync(
            msg.EmailTemplateType,
            msg.Model,
            msg.Language);

        var attachments = new List<EmailAttachment>();

        if (msg.AttachmentFilePaths != null && msg.AttachmentFilePaths.Any())
        {
            foreach (var filePath in msg.AttachmentFilePaths)
            {
                if (File.Exists(filePath))
                {
                    var bytes = await File.ReadAllBytesAsync(filePath);
                    attachments.Add(new EmailAttachment
                    {
                        FileName = Path.GetFileName(filePath),
                        Content = bytes,
                        ContentType = "application/octet-stream"
                    });
                }
            }

            await _emailService.SendEmailWithAttachmentsAsync(msg.ToEmail, msg.Subject, msg.EmailTemplateType, attachments);
        }
    }
}
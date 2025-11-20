using Fluid;
using ItConsultations.Business.Services.EmailService.FluidEmailTemplateService;
using ItConsultations.Business.SharedTypes.Enums.System;
using ItConsultations.Translation;
using Microsoft.AspNetCore.Hosting;

namespace ItConsultations.Business.Services.EmailService.EmailTemplateService;

public class FluidEmailTemplateService
{
    private readonly IWebHostEnvironment _env;
    private readonly ITranslationService _translationService;
    private readonly FluidParser _fluidParser;

    public FluidEmailTemplateService(
        IWebHostEnvironment env,
        ITranslationService translationService
        )
    {
        _env = env;
        _translationService = translationService;
        _fluidParser = new FluidParser();
    }

    public async Task<string> GenerateEmailBodyAsync(EmailTemplateType templateType, object model, string language)
    {
        var path = Path.Combine(_env.ContentRootPath, "EmailTemplates", $"{ templateType.ToString() }Template.liquid");
        var source = await File.ReadAllTextAsync(path);

        if (!_fluidParser.TryParse(source, out var template, out var errors))
        {
            throw new Exception($"Failed to parse email template: { string.Join(", ", errors) }");
        }

        var options = new TemplateOptions();

        options.MemberAccessStrategy.Register(model.GetType());
        options.MemberAccessStrategy.Register<FluidTranslationAccessor, object>((accessor, key) => accessor.Resolve(key));
        var context = new TemplateContext(model, options);
        context.SetValue("T", new FluidTranslationAccessor(_translationService, language));

        return await template.RenderAsync(context);
    }
}

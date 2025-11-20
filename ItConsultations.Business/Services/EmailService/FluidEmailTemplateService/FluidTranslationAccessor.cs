using ItConsultations.Translation;

namespace ItConsultations.Business.Services.EmailService.FluidEmailTemplateService;

public class FluidTranslationAccessor
{
    private readonly ITranslationService _translationService;
    private readonly string _lang;

    public FluidTranslationAccessor(ITranslationService translationService, string lang)
    {
        _translationService = translationService;
        _lang = lang;
    }

    public string Resolve(string key)
    {
        return _translationService.GetTranslation(key, _lang);
    }
}

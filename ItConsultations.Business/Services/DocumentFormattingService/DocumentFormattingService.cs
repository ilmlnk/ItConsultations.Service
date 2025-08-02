using ItConsultations.Translation;

namespace ItConsultations.Business.Services.DocumentFormattingService;

public class DocumentFormattingService
{
    private readonly ITranslationService _translationService;

    public DocumentFormattingService(ITranslationService translationService)
    {
        _translationService = translationService;
    }
}

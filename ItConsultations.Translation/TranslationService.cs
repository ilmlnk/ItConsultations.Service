
namespace ItConsultations.Translation;

public class TranslationService : ITranslationService
{
    private readonly ILogger<TranslationService> _logger;
    private readonly string[] _supportedLangs = new[] { "en", "uk", "fr", "pl", "de", "es", "se", "nb" }; 

    public TranslationService(ILogger<TranslationService> logger)
    {
        _logger = logger;
    }

    public string GetTranslation(string translationKey, string lang)
    {
        throw new NotImplementedException();
    }

    public string GetTranslation(string key, string lang, params object[] args)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetSupportedLanguages()
    {
        throw new NotImplementedException();
    }
}

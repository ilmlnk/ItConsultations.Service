using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ItConsultations.Translation;

public class TranslationService : ITranslationService
{
    private const string _defaultLanguage = "en";
    private readonly string[] _supportedLangs = new[] { "uk", "fr", "pl", "de", "es", "se", "nb" }; 
    private readonly ResourceManager _resourceManager;

    public TranslationService(ResourceManager resourceManager) 
    {
        _resourceManager = new ResourceManager("", Assembly.GetExecutingAssembly());
    }

    public string GetTranslation(string translationKey, string lang)
    {
        var cultureInfo = new CultureInfo(lang);
        return _resourceManager.GetString(translationKey, cultureInfo) ?? translationKey;
    }

    public Dictionary<string, string> GetAllTranslations(string lang)
    {
        var cultureInfo = new CultureInfo(lang);
        var resourceSet = _resourceManager.GetResourceSet(cultureInfo, true, false);

        var translations = new Dictionary<string, string>();

        if (resourceSet != null)
        {
            foreach (DictionaryEntry entry in resourceSet)
            {
                translations.Add(entry.Key.ToString(), entry.Value?.ToString() ?? "");
            }
        }

        return translations;
    }

    public IEnumerable<string> GetSupportedLanguages()
    {
        return _supportedLangs;
    }
}

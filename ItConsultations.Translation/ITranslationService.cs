namespace ItConsultations.Translation;

public interface ITranslationService
{
    string GetTranslation(string translationKey, string lang);

    string GetTranslation(string key, string lang, params object[] args);

    IEnumerable<string> GetSupportedLanguages();
}

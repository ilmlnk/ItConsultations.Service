namespace ItConsultations.Translation;

public interface ITranslationService
{
    string GetTranslation(string translationKey, string lang);

    Dictionary<string, string> GetAllTranslations(string lang);

    IEnumerable<string> GetSupportedLanguages();
}

namespace ItConsultations.Translation.Services;

public interface ITranslationProvider
{
    Task<Dictionary<string, string>> GetTranslationsAsync(string lang);
}

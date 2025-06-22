using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ItConsultations.Translation.Services;

public class TranslationProvider : ITranslationProvider
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public TranslationProvider(
        HttpClient httpClient,
        IMemoryCache cache
        )
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public Task<Dictionary<string, string>> GetTranslationsAsync(string lang)
    {
        throw new NotImplementedException();
    }
}

using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.MetaDtos;
using ItConsultations.Business.Entities.RegionalSettings;
using System.Globalization;

namespace ItConsultations.Business.Services.MetaService;

public class MetaService : IMetaService
{
    private readonly IRepository<Culture, string> _cultureRepository;
    private readonly IRepository<Country, string> _countryRepository;
    private readonly IRepository<Currency, string> _currencyRepository;
    private readonly IRepository<Language, string> _languageRepository;

    public MetaService(
        IRepository<Culture, string> cultureRepository,
        IRepository<Country, string> countryRepository,
        IRepository<Currency, string> currencyRepository,
        IRepository<Language, string> languageRepository)
    {
        _cultureRepository = cultureRepository;
        _countryRepository = countryRepository;
        _currencyRepository = currencyRepository;
        _languageRepository = languageRepository;
    }

    public async Task<List<CultureDto>> GetSupportedCulturesAsync()
    {
        var cultures = await _cultureRepository.GetAllAsync();
        var supportedCultureIds = cultures.Select(c => c.Id);
        var result = new List<CultureDto>();

        foreach (var code in supportedCultureIds)
        {
            var cultureInfo = new CultureInfo(code);
            var regionInfo = new RegionInfo(code);

            result.Add(new CultureDto
            {
                Code = code,
                NativeName = cultureInfo.NativeName,
                DateFormat = cultureInfo.DateTimeFormat.ShortDatePattern,
                CurrencySymbol = regionInfo.CurrencySymbol,
                CurrencyCode = regionInfo.ISOCurrencySymbol
            });
        }

        return result;
    }

    public async Task<List<CountryDto>> GetSupportedCountriesAsync()
    {
        var countries = await _countryRepository.GetAllAsync();
        return countries.Select(c => new CountryDto
        {
            Id = c.Id,
            Name = c.DisplayName
        }).ToList();
    }

    public async Task<List<CurrencyDto>> GetSupportedCurrenciesAsync()
    {
        var currencies = await _currencyRepository.GetAllAsync();
        return currencies.Select(c => new CurrencyDto
        {
            Id = c.Id,
            Name = c.DisplayName,
            Symbol = TryGetCurrencySymbol(c.Id)
        }).ToList();
    }

    public async Task<List<LanguageDto>> GetSupportedLanguagesAsync()
    {
        var languages = await _languageRepository.GetAllAsync();
        return languages.Select(l => new LanguageDto
        {
            Id = l.Id,
            Name = l.DisplayName
        }).ToList();
    }

    private string TryGetCurrencySymbol(string currencyCode)
    {
        var culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .FirstOrDefault(c => new RegionInfo(c.Name).ISOCurrencySymbol == currencyCode);

        if (culture != null)
        {
            return new RegionInfo(culture.Name).CurrencySymbol;
        }

        return currencyCode;
    }
}

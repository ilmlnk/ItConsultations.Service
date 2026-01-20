using ItConsultations.Business.Dtos.MetaDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItConsultations.Business.Services.MetaService;

public interface IMetaService
{
    Task<List<CultureDto>> GetSupportedCulturesAsync();
    Task<List<CountryDto>> GetSupportedCountriesAsync();
    Task<List<CurrencyDto>> GetSupportedCurrenciesAsync();
    Task<List<LanguageDto>> GetSupportedLanguagesAsync();
}

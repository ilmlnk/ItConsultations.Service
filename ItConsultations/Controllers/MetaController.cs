using ItConsultations.Business.Services.MetaService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItConsultations.WebApi.Controllers;

[Route("api/meta")]
public class MetaController : Controller
{
    private readonly IMetaService _metaService;

    public MetaController(IMetaService metaService)
    {
        _metaService = metaService;
    }

    [HttpGet("supported-cultures")]
    public async Task<IActionResult> GetSupportedCultures()
    {
        var result = await _metaService.GetSupportedCulturesAsync();
        return Ok(result);
    }

    [HttpGet("supported-countries")]
    public async Task<IActionResult> GetSupportedCountries()
    {
        var result = await _metaService.GetSupportedCountriesAsync();
        return Ok(result);
    }

    [HttpGet("supported-currencies")]
    public async Task<IActionResult> GetSupportedCurrencies()
    {
        var result = await _metaService.GetSupportedCurrenciesAsync();
        return Ok(result);
    }

    [HttpGet("supported-languages")]
    public async Task<IActionResult> GetSupportedLanguages()
    {
        var result = await _metaService.GetSupportedLanguagesAsync();
        return Ok(result);
    }
}

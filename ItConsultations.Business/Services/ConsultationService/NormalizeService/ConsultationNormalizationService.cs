using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Services.ArticleService.NormalizeService;

namespace ItConsultations.Business.Services.ConsultationService.NormalizationService;

public class ConsultationNormalizationService : IConsultationNormalizationService
{
    private readonly IArticleNormalizationService _articleNormalizationService;

    public ConsultationNormalizationService(IArticleNormalizationService articleNormalizationService)
    {
        _articleNormalizationService = articleNormalizationService;
    }

    public Task<ConsultationDto> NormalizeAsync(ConsultationDto dto)
    {
        throw new NotImplementedException();
    }
}
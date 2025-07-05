using ItConsultations.Business.Dtos.ConsultationDtos;

namespace ItConsultations.Business.Services.ConsultationService.NormalizationService;

public interface IConsultationNormalizationService
{
    Task<ConsultationDto> NormalizeAsync(ConsultationDto dto);
}
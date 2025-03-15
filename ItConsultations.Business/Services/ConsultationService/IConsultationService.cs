using ItConsultations.Business.Dtos;

namespace ItConsultations.Business.Services.ConsultationService;

public interface IConsultationService
{
    Task<ConsultationDto> CreateAsync(ConsultationDto dto);

    Task<ConsultationDto> GetAsync(string consId);

    Task<ConsultationDto> GetAsync(long id);

    Task<List<ConsultationDto>> GetAsync();

    Task<ConsultationDto> UpdateAsync(ConsultationDto dto, string consId);

    Task<ConsultationDto> UpdateAsync(ConsultationDto dto, long id);

    Task<ConsultationDto> DeleteAsync(string consId);
    
    Task<ConsultationDto> DeleteAsync(long id);
}

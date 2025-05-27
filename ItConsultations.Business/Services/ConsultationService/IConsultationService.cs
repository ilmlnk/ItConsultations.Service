using ItConsultations.Business.Dtos.ConsultationDtos;

namespace ItConsultations.Business.Services.ConsultationService;

public interface IConsultationService
{
    Task<ConsultationDto> CreateAsync(CreateConsultationDto dto);

    Task<ConsultationDto> GetAsync(string consId);

    Task<ConsultationDto> GetAsync(long id);

    Task<List<ConsultationDto>> GetAsync();

    Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, string consId);

    Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, long id);

    Task<ConsultationDto> DeleteAsync(string consId);
    
    Task<ConsultationDto> DeleteAsync(long id);
}

using ItConsultations.Business.Dtos;

namespace ItConsultations.Business.Services.Consultation;

public interface IConsultationService
{
    Task<ConsultationDto> CreateAsync(ConsultationDto dto);
    Task<ConsultationDto> GetAsync(string id);
    Task<List<ConsultationDto>> GetAsync();
    Task<ConsultationDto> UpdateAsync(ConsultationDto dto, string id);
    Task<ConsultationDto> DeleteAsync(string id);

}

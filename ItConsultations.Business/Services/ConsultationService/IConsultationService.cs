using ItConsultations.Business.Dtos.ConsultationDtos;

namespace ItConsultations.Business.Services.ConsultationService;

public interface IConsultationService
{
    Task<ConsultationDto> CreateAsync(CreateConsultationDto dto);

    Task<ConsultationDto> CreateAsync(CreateConsultationDto dto, string consId);

    Task<ConsultationDto> GetAsync(long id);

    ConsultationDto Get(string consId);

    Task<IEnumerable<ConsultationDto>> GetAllAsync();

    Task<IEnumerable<ConsultationDto>> GetByCoachConsIdAsync(string coachConsId);

    Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, string consId);

    Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, long id);
    
    Task<ConsultationDto> DeleteAsync(DeleteConsultationDto dto, long consultationId);

    Task<IEnumerable<ConsultationDto>> DeleteForUserAsync(string userConsId);
}

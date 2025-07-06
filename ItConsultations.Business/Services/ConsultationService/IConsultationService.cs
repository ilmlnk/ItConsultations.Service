using ItConsultations.Business.Dtos.ConsultationDtos;

namespace ItConsultations.Business.Services.ConsultationService;

public interface IConsultationService
{
    Task<ConsultationDto> CreateAsync(CreateConsultationDto dto);

    Task<ConsultationDto> CreateAsync(CreateConsultationDto dto, string consId);

    Task<ConsultationDto> GetAsync(string consId);

    Task<ConsultationDto> GetAsync(long id);

    Task<IEnumerable<ConsultationDto>> GetAllAsync();

    Task<IEnumerable<ConsultationDto>> GetByCoachConsIdAsync(string coachConsId);

    Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, string consId);

    Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, long id);

    Task<ConsultationDto> DeleteAsync(long id);

    Task<ConsultationDto> DeleteAsync(string consId);
    
    Task<ConsultationDto> DeleteAsync(DeleteConsultationDto dto, long id);

    Task<IEnumerable<ConsultationDto>> DeleteForUserAsync(string userConsId);
}

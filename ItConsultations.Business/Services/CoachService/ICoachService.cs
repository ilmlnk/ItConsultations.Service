using ItConsultations.Business.Dtos.CoachDtos;

namespace ItConsultations.Business.Services.CoachService;

public interface ICoachService
{
    Task<CoachDto> CreateAsync(CreateCoachDto dto);

    Task<CoachDto> UpdateAsync(UpdateCoachDto dto);

    Task<CoachDto> DeleteAsync(long id);

    CoachDto GetCoach(string coachConsId);

    Task<CoachDto> GetAsync(long id);

    Task<IEnumerable<CoachDto>> GetAllAsync();
}

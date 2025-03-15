using ItConsultations.Business.Dtos;

namespace ItConsultations.Business.Services.CoachService;

public interface ICoachService
{
    Task<CoachDto> CreateAsync(CoachDto dto);

    Task<CoachDto> UpdateAsync(CoachDto dto);

    Task<CoachDto> DeleteAsync(long id);
}

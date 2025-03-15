

using ItConsultations.Business.Dtos;

namespace ItConsultations.Business.Services.Coach;

public interface ICoachService
{
    Task<CoachDto> CreateAsync(CoachDto dto);
    Task<CoachDto> DeleteAsync(long id);
}

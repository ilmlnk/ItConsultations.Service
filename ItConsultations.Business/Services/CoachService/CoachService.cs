using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Utilities.Guards;

namespace ItConsultations.Business.Services.CoachService;

public class CoachService : ICoachService
{
    private readonly IRepository<Coach, long> _repository;

    public CoachService(IRepository<Coach, long> repository)
    {
        _repository = repository;
    }

    public async Task<CoachDto> CreateAsync(CreateCoachDto dto)
    {
        var coach = MapperManager.Map<Coach>(dto);
        coach.CoachConsId = IdGeneratorService.IdGeneratorService.GenerateCoachId();
        await _repository.CreateAsync(coach);
        return MapperManager.Map<CoachDto>(coach);
    }

    public async Task<CoachDto> DeleteAsync(long id)
    {
        var coach = await _repository.GetAsync(id);

        if (coach == null)
        {
            return null;
        }
        
        await _repository.DeleteAsync(coach);
        return MapperManager.Map<CoachDto>(coach);
    }

    public async Task<IEnumerable<CoachDto>> GetAllAsync()
    {
        var coaches = await _repository.GetAllAsync();
        return MapperManager.Map<List<CoachDto>>(coaches);
    }

    public async Task<CoachDto> GetAsync(long id)
    {
        var coach = await _repository.GetAsync(id);
        return coach != null ? MapperManager.Map<CoachDto>(coach) : null;
    }

    public CoachDto GetByCoachConsId(string coachConsId)
    {
        var coach = _repository.Get(c => c.CoachConsId == coachConsId).FirstOrDefault();
        return coach != null ? MapperManager.Map<CoachDto>(coach) : null;
    }

    public async Task<CoachDto> UpdateAsync(UpdateCoachDto dto)
    {
        var existingCoach = await _repository.GetAsync(dto.Id);
        Guard.NotNull(existingCoach, nameof(existingCoach));

        existingCoach = MapperManager.Map<Coach>(dto);
        await _repository.UpdateAsync(existingCoach);

        return MapperManager.Map<CoachDto>(existingCoach);
    }
}

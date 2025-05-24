using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Entities.Consultation;
using System.Data.Entity;

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
        var originalDto = MapperManager.Map<CoachDto>(dto);
        var coach = MapperManager.Map<Coach>(originalDto);
        await _repository.CreateAsync(coach);
        var coachDto = MapperManager.Map<CoachDto>(coach);
        return coachDto;
    }

    public async Task<CoachDto> DeleteAsync(long id)
    {
        var coach = await _repository.GetAsync(id);
        var coachDto = MapperManager.Map<CoachDto>(coach);
        await _repository.DeleteAsync(coach);
        return coachDto;
    }

    public async Task<IEnumerable<CoachDto>> GetAllAsync()
    {
        var coaches = await _repository.Get(x => true).ToListAsync();
        return MapperManager.Map<List<CoachDto>>(coaches);
    }

    public async Task<CoachDto> GetAsync(long id)
    {
        var coach = await _repository.GetAsync(id);
        var dto = MapperManager.Map<CoachDto>(coach);
        return dto;
    }

    public async Task<CoachDto> UpdateAsync(UpdateCoachDto dto)
    {
        var existingCoach = await _repository.GetAsync(dto.Id);
        var coachDto = MapperManager.Map<Coach, CoachDto>(existingCoach);
        return coachDto;
    }
}

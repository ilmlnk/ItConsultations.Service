using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos;
using ItConsultations.Business.Entities.Consultation;

namespace ItConsultations.Business.Services.CoachService;


public class CoachService : ICoachService
{
    private readonly IRepository<Coach, long> _repository;

    public CoachService(IRepository<Coach, long> repository)
    {
        _repository = repository;
    }

    public async Task<CoachDto> CreateAsync(CoachDto dto)
    {
        return null;
    }

    public async Task<CoachDto> DeleteAsync(long id)
    {
        var coach = await _repository.GetAsync(id);
        await _repository.DeleteAsync(coach);

        return null;
    }

    public async Task<IEnumerable<CoachDto>> GetAllAsync()
    {
        var coaches = _repository.Get(c => true);

        return await Task.FromResult(coaches.Select(coach => new CoachDto
        {
            Id = coach.Id,
            FirstName = coach.FirstName,
            LastName = coach.LastName,
            CoachConsId = coach.CoachConsId,
            BirthDate = coach.BirthDate,
            Email = coach.Email,
            Username = coach.Username,
            Password = coach.Password,
            LinkedInUrl = coach.LinkedInUrl,
            GitHubUrl = coach.GitHubUrl
        }));
    }

    public Task<CoachDto> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<CoachDto> UpdateAsync(CoachDto dto)
    {
        throw new NotImplementedException();
    }
}

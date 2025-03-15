using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos;
using ItConsultations.Business.Entities.Consultation;

namespace ItConsultations.Business.Services.ConsultationService;

public class ConsultationService : IConsultationService
{
    private readonly IRepository<Consultation, long> _repository;
    public ConsultationService(IRepository<Consultation, long> repository)
    {
        _repository = repository;
    }

    public Task<ConsultationDto> CreateAsync(ConsultationDto dto)
    {
        throw new NotImplementedException();
        // var dto = MapperManager.
    }

    public Task<ConsultationDto> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ConsultationDto>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> UpdateAsync(ConsultationDto dto, string id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> UpdateAsync(ConsultationDto dto, long id)
    {
        throw new NotImplementedException();
    }
}

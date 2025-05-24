using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ConsultationDtos;
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
        var consultationDto = MapperManager.Map<ConsultationDto>(dto);
        return Task.FromResult(consultationDto);
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

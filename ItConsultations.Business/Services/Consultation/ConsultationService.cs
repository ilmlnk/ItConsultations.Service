using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos;

namespace ItConsultations.Business.Services.Consultation;

public class ConsultationService : IConsultationService
{
    private readonly IRepository<Entities.Consultation.Consultation, long> _repository;
    public ConsultationService(
        IRepository<Entities.Consultation.Consultation, long> repository
        )
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

    public Task<ConsultationDto> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ConsultationDto>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> UpdateAsync(ConsultationDto dto, string id)
    {
        throw new NotImplementedException();
    }
}

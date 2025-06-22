using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Entities.Consultation;
using System.Data.Entity;

namespace ItConsultations.Business.Services.ConsultationService;

public class ConsultationService : IConsultationService
{
    private readonly IRepository<Consultation, long> _repository;
    public ConsultationService(IRepository<Consultation, long> repository)
    {
        _repository = repository;
    }

    public async Task<ConsultationDto> CreateAsync(CreateConsultationDto dto)
    {
        var originalDto = MapperManager.Map<ConsultationDto>(dto);
        var consultation = MapperManager.Map<Consultation>(originalDto);
        consultation = await _repository.CreateAsync(consultation);
        var consultationDto = MapperManager.Map<ConsultationDto>(consultation);
        return consultationDto;
    }

    public Task<ConsultationDto> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> DeleteAsync(DeleteConsultationDto dto, long id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ConsultationDto>> GetAsync()
    {
        var consultations = await _repository.Get(x => true).ToListAsync();
        return MapperManager.Map<List<ConsultationDto>>(consultations);
    }

    public Task<ConsultationDto> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, string id)
    {
        throw new NotImplementedException();
    }

    public Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, long id)
    {
        throw new NotImplementedException();
    }
}

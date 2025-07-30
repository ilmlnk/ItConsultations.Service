using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Entities.Consultations;
using ItConsultations.Business.Exceptions;

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
        var consultation = MapperManager.Map<Consultation>(dto);
        consultation.ConsId = IdGeneratorService.IdGeneratorService.GenerateConsultationId();
        await _repository.CreateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> CreateAsync(CreateConsultationDto dto, string coachConsId)
    {
        var consultation = MapperManager.Map<Consultation>(dto);
        consultation.ConsId = IdGeneratorService.IdGeneratorService.GenerateConsultationId();
        consultation.Coach.CoachConsId = coachConsId;
        consultation = await _repository.CreateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> DeleteAsync(DeleteConsultationDto dto, long consultationId)
    {
        var consultation = await _repository.GetAsync(consultationId);
        
        if (consultation == null)
        {
            throw new ConsultationsNotFoundException();
        }

        await _repository.DeleteAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<IEnumerable<ConsultationDto>> DeleteForUserAsync(string userConsId)
    {
        var consultations = _repository.Get(c => c.Coach.CoachConsId == userConsId).ToList();
        await _repository.DeleteAsync(consultations);
        return MapperManager.Map<IEnumerable<ConsultationDto>>(consultations);
    }

    public ConsultationDto Get(string consId)
    {
        var consultation = _repository.Get(x => x.ConsId == consId).FirstOrDefault();
        return consultation != null ? MapperManager.Map<ConsultationDto>(consultation) : null;
    }

    public async Task<IEnumerable<ConsultationDto>> GetAllAsync()
    {
        var consultations = await _repository.GetAllAsync();
        return MapperManager.Map<IEnumerable<ConsultationDto>>(consultations);
    }

    public async Task<ConsultationDto> GetAsync(long id)
    {
        var consultation = await _repository.GetAsync(id);
        return consultation != null ? MapperManager.Map<ConsultationDto>(consultation) : null;
    }

    public async Task<IEnumerable<ConsultationDto>> GetByCoachConsIdAsync(string coachConsId)
    {
        var consultations = _repository
            .Get(x => x.Coach.CoachConsId == coachConsId)
            .ToList();

        return MapperManager.Map<List<ConsultationDto>>(consultations);
    }

    public async Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, string id)
    {
        var consultation = _repository.Get(c => c.ConsId == id).FirstOrDefault();
        
        if (consultation == null)
        {
            throw new ConsultationsNotFoundException();
        }

        await _repository.UpdateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, long id)
    {
        var consultation = await _repository.GetAsync(id);
        
        if (consultation == null)
        {
            throw new ConsultationsNotFoundException();
        }

        await _repository.UpdateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }
}

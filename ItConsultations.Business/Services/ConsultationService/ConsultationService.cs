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

    public async Task<ConsultationDto> CreateAsync(CreateConsultationDto dto)
    {
        var consultation = MapperManager.Map<Consultation>(dto);
        consultation.ConsId = GenerateConsultationId();
        consultation = await _repository.CreateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> CreateAsync(CreateConsultationDto dto, string coachConsId)
    {
        var consultation = MapperManager.Map<Consultation>(dto);
        consultation.Coach.CoachConsId = coachConsId;
        consultation = await _repository.CreateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> DeleteAsync(string consId)
    {
        var consultation = _repository.Get(c => c.ConsId == consId).FirstOrDefault();

        if (consultation == null)
        {
            return null;
        }

        await _repository.DeleteAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> DeleteAsync(long id)
    {
        var consultation = await _repository.GetAsync(id);
        
        if (consultation == null)
        {
            return null;
        }

        await _repository.DeleteAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> DeleteAsync(DeleteConsultationDto dto, long id)
    {
        var consultation = await _repository.GetAsync(id);
        
        if (consultation == null)
        {
            return null;
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

    public async Task<ConsultationDto> GetAsync(string consId)
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
            return null;
        }

        await _repository.UpdateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    public async Task<ConsultationDto> UpdateAsync(UpdateConsultationDto dto, long id)
    {
        var consultation = await _repository.GetAsync(id);
        
        if (consultation == null)
        {
            return null;
        }

        await _repository.UpdateAsync(consultation);
        return MapperManager.Map<ConsultationDto>(consultation);
    }

    // to generate consultation id it is used 0002 prefix
    private string GenerateConsultationId()
    {
        return $"0002{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";
    }
}

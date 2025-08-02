using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Entities.Consultations;

namespace ItConsultations.Business.Services.AccessValidation.ConsultationValidation.ConsultationProvider;

public class ConsultationInfoService : IConsultationInfoService
{
    private readonly IRepository<Consultation, long> _repository;

    public ConsultationInfoService()
    {

    }
}

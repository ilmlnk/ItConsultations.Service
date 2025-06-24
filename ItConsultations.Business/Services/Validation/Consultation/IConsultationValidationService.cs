using ItConsultations.Business.Dtos.ConsultationDtos;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Services.Validation.Consultation;

public interface IConsultationValidationService
{
    ValidationResult Validate(ConsultationDto dto);
}

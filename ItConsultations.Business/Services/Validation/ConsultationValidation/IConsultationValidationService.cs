using ItConsultations.Business.Dtos.ConsultationDtos;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Services.AccessValidation.ConsultationValidation;

public interface IConsultationValidationService
{
    ValidationResult Validate(ConsultationDto dto);
}

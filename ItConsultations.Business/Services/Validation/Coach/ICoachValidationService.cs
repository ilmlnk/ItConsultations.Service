using ItConsultations.Business.Dtos;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Services.Validation.Coach;

public interface ICoachValidationService
{
    ValidationResult Validate(CoachDto dto);
}

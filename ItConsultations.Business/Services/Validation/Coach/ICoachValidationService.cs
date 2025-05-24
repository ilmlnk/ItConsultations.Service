using ItConsultations.Business.Dtos.CoachDtos;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Services.Validation.Coach;

public interface ICoachValidationService
{
    ValidationResult Validate(CoachDto dto);
}

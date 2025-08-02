using ItConsultations.Business.Dtos.CoachDtos;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Services.AccessValidation.CoachValidation;

public interface ICoachValidationService
{
    ValidationResult Validate(CoachDto dto);
}

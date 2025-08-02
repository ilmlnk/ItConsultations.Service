using ItConsultations.Business.Dtos.StudentDtos;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Services.AccessValidation.StudentValidation;

public interface IStudentValidationService
{
    ValidationResult Validate(StudentDto dto);
}

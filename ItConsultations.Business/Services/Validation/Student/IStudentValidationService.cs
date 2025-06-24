using ItConsultations.Business.Dtos.StudentDtos;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Services.Validation.Student;

public interface IStudentValidationService
{
    ValidationResult Validate(StudentDto dto);
}

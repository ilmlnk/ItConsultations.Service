using ItConsultations.Business.Dtos.StudentDtos;

namespace ItConsultations.Business.Services.StudentService.NormalizationService;

public interface IStudentNormalizationService
{
    Task<StudentDto> NormalizeAsync(StudentDto dto);
}
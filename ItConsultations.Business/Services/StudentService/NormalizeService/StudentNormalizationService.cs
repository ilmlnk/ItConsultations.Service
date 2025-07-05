using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Services.ConsultationService.NormalizationService;

namespace ItConsultations.Business.Services.StudentService.NormalizationService;

public class StudentNormalizationService : IStudentNormalizationService
{
    private readonly IConsultationNormalizationService _consultationNormalizationService;

    public StudentNormalizationService(IConsultationNormalizationService consultationNormalizationService)
    {
        _consultationNormalizationService = consultationNormalizationService;
    }

    public Task<StudentDto> NormalizeAsync(StudentDto dto)
    {
        throw new NotImplementedException();
    }
}
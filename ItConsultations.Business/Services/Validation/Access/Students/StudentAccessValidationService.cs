using ItConsultations.Business.Services.StudentService;
using ItConsultations.Utilities.Guards;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.Access.Students;

public class StudentAccessValidationService : AccessValidationServiceBase, IStudentAccessValidationService
{
    private readonly IStudentService _studentService;
    public StudentAccessValidationService(
        IStudentService studentService
        )
    {
        _studentService = studentService;
    }

    public async void ValidateStudentAccessAsync(long id)
    {
        var student = await _studentService.GetByIdAsync(id);

        Guard.NotNull(student);
        Guard.That(student.Username == null, "Student does not have a specified username.");
    }
}

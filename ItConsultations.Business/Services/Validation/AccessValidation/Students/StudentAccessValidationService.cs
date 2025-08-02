using ItConsultations.Business.Services.StudentService;
using ItConsultations.Utilities.Guards;
using ItConsultations.Utilities.Validation.Access;

namespace ItConsultations.Business.Services.Validation.AccessValidation.Students;

public class StudentAccessValidationService : AccessValidationServiceBase, IStudentAccessValidationService
{
    private readonly IStudentService _studentService;
    public StudentAccessValidationService(
        IStudentService studentService
        )
    {
        _studentService = studentService;
    }

    public void ValidateStudentAccessAsync(long id)
    {
        var student = _studentService.GetAsync(id).Result;

        if (student == null)
        {
            throw new ArgumentException($"Student with id {id} not found");
        }

        Guard.That(student.Username == null, "Student does not have a specified username.");
    }
}

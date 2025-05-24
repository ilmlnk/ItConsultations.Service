using ItConsultations.Business.Dtos.StudentDtos;

namespace ItConsultations.Business.Services.Student;

public interface IStudentService
{
    Task<StudentDto> CreateAsync(CreateStudentDto dto);

    Task<StudentDto> CreateAsync(CreateStudentDto dto, string id);

    Task<StudentDto> UpdateAsync(StudentDto dto, string id);

    Task<StudentDto> GetByIdAsync(string id);

    Task<IEnumerable<StudentDto>> GetAllAsync();

    Task<StudentDto> DeleteAsync(int id);
}

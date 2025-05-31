using ItConsultations.Business.Dtos.StudentDtos;

namespace ItConsultations.Business.Services.StudentService;

public interface IStudentService
{
    Task<StudentDto> CreateAsync(CreateStudentDto dto);

    Task<StudentDto> CreateAsync(CreateStudentDto dto, string id);

    Task<StudentDto> UpdateAsync(UpdateStudentDto dto, string id);

    Task<StudentDto> UpdateAsync(UpdateStudentDto dto, long id);

    Task<StudentDto> GetByIdAsync(long id);

    Task<StudentDto> GetByIdAsync(string id);

    Task<IEnumerable<StudentDto>> GetAllAsync();

    Task DeleteAsync(long id);

    Task DeleteAsync(string id);
}

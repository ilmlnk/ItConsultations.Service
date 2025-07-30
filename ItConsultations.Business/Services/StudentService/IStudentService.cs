using ItConsultations.Business.Dtos.StudentDtos;

namespace ItConsultations.Business.Services.StudentService;

public interface IStudentService
{
    Task<StudentDto> CreateAsync(CreateStudentDto dto);

    Task<StudentDto> UpdateAsync(UpdateStudentDto dto, string id);

    Task<StudentDto> UpdateAsync(UpdateStudentDto dto, long id);

    Task<StudentDto> GetAsync(long id);

    Task<StudentDto> GetAsync(string id);

    Task<IEnumerable<StudentDto>> GetAllAsync();

    Task DeleteAsync(long id);

    Task DeleteAsync(string studentConsId);
}

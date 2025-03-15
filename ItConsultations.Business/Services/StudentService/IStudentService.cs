using ItConsultations.Business.Dtos;

namespace ItConsultations.Business.Services.Student;

public interface IStudentService
{
    Task<StudentDto> CreateAsync(StudentDto dto);

    Task<StudentDto> UpdateAsync(StudentDto dto, string id);

    Task<StudentDto> GetByIdAsync(int id);

    Task<IEnumerable<StudentDto>> GetAllAsync();

    Task<StudentDto> DeleteAsync(int id);
}

using ItConsultations.Business.Dtos.StudentDtos;

namespace ItConsultations.Business.Services.Student;

public class StudentService : IStudentService
{
    public Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> CreateAsync(CreateStudentDto dto, string id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> UpdateAsync(StudentDto dto, string id)
    {
        throw new NotImplementedException();
    }
}

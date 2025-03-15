using ItConsultations.Business.Dtos;

namespace ItConsultations.Business.Services.Student;

public class StudentService : IStudentService
{
    public Task CreateAsync(StudentDto dto)
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

    public Task<StudentDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> UpdateAsync(StudentDto dto, string id)
    {
        throw new NotImplementedException();
    }

    Task<StudentDto> IStudentService.CreateAsync(StudentDto dto)
    {
        throw new NotImplementedException();
    }
}

using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Entities.Consultation;

namespace ItConsultations.Business.Services.StudentService;

public class StudentService : IStudentService
{
    private readonly IRepository<Student, long> _repository;

    public StudentService(IRepository<Student, long> repository)
    {
        _repository = repository;
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var originalDto = MapperManager.Map<StudentDto>(dto);
        var student = MapperManager.Map<Student>(originalDto);
        await _repository.CreateAsync(student);
        var studentDto = MapperManager.Map<StudentDto>(student);
        return studentDto;
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto, string id)
    {
        var originalDto = MapperManager.Map<StudentDto>(dto);
        originalDto.StudentConsId = id;

        var student = MapperManager.Map<Student>(originalDto);
        await _repository.CreateAsync(student);
        var studentDto = MapperManager.Map<StudentDto>(student);
        return studentDto;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = _repository
            .Include(student => student.Consultation)
            .SingleOrDefault(student => student.Id == id);

        if (entity == null)
        {
            return;
        }

        await _repository.DeleteAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        var entity = _repository
            .Include(student => student.Consultation)
            .SingleOrDefault(student => student.StudentConsId == id);

        if (entity == null)
        {
            return;
        }

        await _repository.DeleteAsync(entity);
    }

    public Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> UpdateAsync(UpdateStudentDto dto, string id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentDto> UpdateAsync(UpdateStudentDto dto, long id)
    {
        throw new NotImplementedException();
    }
}

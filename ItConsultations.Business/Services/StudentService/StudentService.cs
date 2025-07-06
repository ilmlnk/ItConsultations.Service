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
        var student = MapperManager.Map<Student>(dto);
        student.StudentConsId = GenerateStudentId();
        student.PictureUrl = string.Empty;
        await _repository.CreateAsync(student);
        var studentDto = MapperManager.Map<StudentDto>(student);
        return studentDto;
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto, string id)
    {
        var student = MapperManager.Map<Student>(dto);
        student.StudentConsId = id;
        student.PictureUrl = string.Empty;
        await _repository.CreateAsync(student);
        var studentDto = MapperManager.Map<StudentDto>(student);
        return studentDto;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = _repository
            .Get(student => student.Id == id)
            .SingleOrDefault();

        if (entity == null)
        {
            return;
        }

        await _repository.DeleteAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        var entity = _repository
            .Get(student => student.StudentConsId == id)
            .SingleOrDefault();

        if (entity == null)
        {
            return;
        }

        await _repository.DeleteAsync(entity);
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = _repository
            .Get(s => true)
            .ToList();
        
        return MapperManager.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<StudentDto> GetByIdAsync(string id)
    {
        var student = _repository
            .Get(s => s.StudentConsId == id)
            .FirstOrDefault();
        
        return student != null ? MapperManager.Map<StudentDto>(student) : null;
    }

    public async Task<StudentDto> GetByIdAsync(long id)
    {
        var student = _repository
            .Get(s => s.Id == id)
            .FirstOrDefault();
        
        return student != null ? MapperManager.Map<StudentDto>(student) : null;
    }

    public async Task<StudentDto> UpdateAsync(UpdateStudentDto dto, string id)
    {
        var existingStudent = _repository
            .Get(s => s.StudentConsId == id)
            .FirstOrDefault();
        
        if (existingStudent == null)
        {
            return null;
        }
        
        existingStudent.FirstName = dto.FirstName;
        existingStudent.LastName = dto.LastName;
        existingStudent.Email = dto.Email;
        existingStudent.LinkedInUrl = dto.LinkedInUrl;
        existingStudent.GitHubUrl = dto.GitHubUrl;
        
        await _repository.UpdateAsync(existingStudent);
        
        return MapperManager.Map<StudentDto>(existingStudent);
    }

    public async Task<StudentDto> UpdateAsync(UpdateStudentDto dto, long id)
    {
        var existingStudent = _repository
            .Get(s => s.Id == id)
            .FirstOrDefault();
        
        if (existingStudent == null)
        {
            return null;
        }
        
        existingStudent.FirstName = dto.FirstName;
        existingStudent.LastName = dto.LastName;
        existingStudent.Email = dto.Email;
        existingStudent.LinkedInUrl = dto.LinkedInUrl;
        existingStudent.GitHubUrl = dto.GitHubUrl;
        
        await _repository.UpdateAsync(existingStudent);
        
        return MapperManager.Map<StudentDto>(existingStudent);
    }

    // to generate student id it is used 0003 prefix
    private string GenerateStudentId()
    {
        return $"0003{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";
    }
}

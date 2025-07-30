using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Entities.Students;
using ItConsultations.Business.Exceptions;

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
        student.StudentConsId = IdGeneratorService.IdGeneratorService.GenerateStudentId();
        student.PictureUrl = string.Empty;
        var studentDto = MapperManager.Map<StudentDto>(student);
        await _repository.CreateAsync(student);
        return studentDto;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = _repository
            .Get(student => student.Id == id)
            .SingleOrDefault();

        if (entity == null)
        {
            throw new ConsultationsNotFoundException();
        }

        await _repository.DeleteAsync(entity);
    }

    public async Task DeleteAsync(string studentConsId)
    {
        var entity = _repository
            .Get(student => student.StudentConsId == studentConsId)
            .SingleOrDefault();

        if (entity == null)
        {
            throw new ConsultationsNotFoundException();
        }

        await _repository.DeleteAsync(entity);
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = _repository
            .Get(s => true)
            .ToList();

        return students.Select(MapperManager.Map<StudentDto>).ToList();
    }

    public async Task<StudentDto> GetAsync(string studentConsId)
    {
        var student = _repository
            .Get(s => s.StudentConsId == studentConsId)
            .FirstOrDefault();
        
        return student != null ? MapperManager.Map<StudentDto>(student) : null;
    }

    public async Task<StudentDto> GetAsync(long id)
    {
        var student = _repository
            .Get(s => s.Id == id)
            .FirstOrDefault();
        
        return student != null ? MapperManager.Map<StudentDto>(student) : null;
    }

    public async Task<StudentDto> UpdateAsync(UpdateStudentDto dto, string id)
    {
        var originalStudent = _repository
            .Get(s => s.StudentConsId == id)
            .FirstOrDefault();
        
        if (originalStudent == null)
        {
            throw new ConsultationsNotFoundException(); // TODO: add parameters for the exception
        }

        var updatedStudent = MapperManager.Map(dto, originalStudent);
        await _repository.UpdateAsync(updatedStudent);
        
        return MapperManager.Map<StudentDto>(updatedStudent);
    }

    public async Task<StudentDto> UpdateAsync(UpdateStudentDto dto, long id)
    {
        var existingStudent = _repository
            .Get(s => s.Id == id)
            .FirstOrDefault();

        if (existingStudent == null)
        {
            throw new ConsultationsNotFoundException();
        }

        var mappedStudent = MapperManager.Map(dto, existingStudent);
        await _repository.UpdateAsync(mappedStudent);
        var studentDto = MapperManager.Map<StudentDto>(mappedStudent);
        return MapperManager.Map<StudentDto>(studentDto);
    }
}

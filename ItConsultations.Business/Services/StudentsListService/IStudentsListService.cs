using ItConsultations.Business.Dtos.StudentDtos;

namespace ItConsultations.Business.Services.StudentsListService;

public interface IStudentsListService
{
    Task<StudentListDto> GetStudentListItemsAsync(); 
}

namespace ItConsultations.Business.Dtos.StudentDtos;

public class StudentListDto
{
    public IEnumerable<StudentListItemDto> StudentList { get; set; }

    public int TotalNumber { get; set; }
}

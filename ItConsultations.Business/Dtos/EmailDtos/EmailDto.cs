namespace ItConsultations.Business.Dtos.EmailDtos;

public class EmailDto
{
    public string Subject { get; set; }

    public string Body { get; set; }

    public IEnumerable<Guid> EmailIds { get; set; } = new List<Guid>();
}

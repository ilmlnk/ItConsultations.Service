namespace ItConsultations.Business.Dtos.EventDtos;
public class ExportUserEventsToGoogleDto
{
    public string UserAccessToken { get; set; } = string.Empty;

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}

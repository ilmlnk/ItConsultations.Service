namespace ItConsultations.Business.Dtos.GoogleCalendarDtos;

public class GoogleCalendarAttendeeDto
{
    public string Email { get; set; } = string.Empty;

    public string? DisplayName { get; set; }

    public string? ResponseStatus { get; set; }

    public bool? Optional { get; set; }
}

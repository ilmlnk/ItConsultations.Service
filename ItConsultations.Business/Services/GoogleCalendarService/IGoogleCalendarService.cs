using ItConsultations.Business.Dtos.EventDtos;
using ItConsultations.Business.Entities.Events;

namespace ItConsultations.Business.Services.GoogleCalendarService;

public interface IGoogleCalendarService
{
    Task<string> GetAuthUrlAsync(string redirectUri);

    Task<string> GetAccessTokenAsync(string authorizationCode, string redirectUri);

    Task<bool> RefreshAccessTokenAsync(string refreshToken);

    Task<string> CreateEventAsync(Event eventEntity);

    Task<bool> UpdateEventAsync(Event eventEntity);

    Task<bool> DeleteEventAsync(string googleEventId, string calendarId = "primary");

    Task<GoogleCalendarEventDto?> GetEventAsync(string googleEventId, string calendarId = "primary");

    Task<IEnumerable<GoogleCalendarEventDto>> GetEventsAsync(DateTime? fromDate = null, DateTime? toDate = null, string calendarId = "primary");

    Task<IEnumerable<GoogleCalendarEventDto>> GetUpcomingEventsAsync(int maxResults = 10, string calendarId = "primary");

    Task<bool> SyncEventToGoogleCalendarAsync(Event eventEntity);

    Task<Event?> SyncEventFromGoogleCalendarAsync(string googleEventId, long creatorId);

    Task<bool> SendInvitationAsync(string googleEventId, string email, string calendarId = "primary");

    Task<bool> UpdateEventRemindersAsync(string googleEventId, int reminderMinutes, string calendarId = "primary");

    Task<string> CreateRecurringEventAsync(Event eventEntity);

    Task<bool> UpdateRecurringEventAsync(Event eventEntity);

    Task<bool> DeleteRecurringEventAsync(string googleEventId, string calendarId = "primary");

    Task<bool> IsEventExistsAsync(string googleEventId, string calendarId = "primary");

    // Export functionality
    Task<string> ExportEventToICalendarAsync(Event eventEntity);

    Task<string> ExportEventsToICalendarAsync(IEnumerable<Event> events);

    Task<string> GetGoogleCalendarImportUrlAsync(IEnumerable<Event> events);

    Task<bool> ExportEventToGoogleCalendarAsync(Event eventEntity, string userAccessToken);

    Task<bool> ExportEventsToGoogleCalendarAsync(IEnumerable<Event> events, string userAccessToken);

    Task<string> GetCalendarIdAsync(string calendarName = "primary");

    Task<IEnumerable<string>> GetAvailableCalendarsAsync();
    Task<bool> ExportEventAsync(Event? eventEntity, string userAccessToken);
}
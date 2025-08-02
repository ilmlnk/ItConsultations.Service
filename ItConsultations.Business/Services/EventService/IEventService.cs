using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Dtos.EventDtos;
using ItConsultations.Business.SharedTypes.Enums.Event;

namespace ItConsultations.Business.Services.EventService;

public interface IEventService
{
    Task<EventDto> CreateAsync(CreateEventDto createDto, string creatorId);
    
    Task<EventDto> UpdateAsync(UpdateEventDto updateDto);
    
    Task<bool> DeleteAsync(long eventId);
    
    Task<EventDto?> GetByIdAsync(long eventId);
    
    Task<EventDto?> GetByConsIdAsync(string eventConsId);
    
    Task<IEnumerable<EventDto>> SearchAsync(EventSearchDto searchDto);
    
    Task<IEnumerable<EventDto>> GetUserEventsAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null);
    
    Task<IEnumerable<EventDto>> GetUpcomingEventsAsync(int days = 7);
    
    Task<EventDto> AddParticipantAsync(string eventId, long userId, AddParticipantDto dto);
    
    Task<bool> RemoveParticipantAsync(string eventId, long userId);
    
    Task<EventDto> UpdateParticipantStatusAsync(string eventId, long userId, ParticipantStatus status, string? comment = null);
    
    Task<EventDto> AddAttachmentAsync(string eventId, long attachmentId, string? description = null);
    
    Task<bool> RemoveAttachmentAsync(string eventId, long attachmentId);
    
    Task<EventDto> UpdateRecurrenceAsync(string eventId, RecurrenceType recurrenceType, int? interval = null, DateTime? endDate = null);
    
    Task<IEnumerable<EventDto>> GetRecurringEventsAsync(string eventId);
    
    Task<EventDto> CancelEventAsync(string eventId, string? reason = null);
    
    Task<EventDto> RescheduleEventAsync(string eventId, DateTime newBeginDateTime, DateTime newEndDateTime);
    
    Task<bool> SendInvitationsAsync(string eventId);
    
    Task<bool> SendRemindersAsync(string eventId);
    
    Task<EventDto> SyncWithGoogleCalendarAsync(string eventId);
    
    Task<EventDto> CreateFromGoogleCalendarAsync(GoogleCalendarEventDto googleEvent, string creatorId);
    
    Task<bool> UpdateGoogleCalendarEventAsync(string eventId);
    
    Task<bool> DeleteGoogleCalendarEventAsync(string eventId);
    
    // Export functionality
    Task<string> ExportEventToICalendarAsync(string eventId);
    
    Task<string> ExportUserEventsToICalendarAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null);
    
    Task<string> ExportEventsToICalendarAsync(IEnumerable<long> eventIds);
    
    Task<string> GetGoogleCalendarImportUrlAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null);
    
    Task<string> GetGoogleCalendarImportUrlAsync(IEnumerable<long> eventIds);
    
    Task<bool> ExportEventToGoogleCalendarAsync(string eventId, string userAccessToken);
    
    Task<bool> ExportUserEventsToGoogleCalendarAsync(string userId, string userAccessToken, DateTime? fromDate = null, DateTime? toDate = null);
} 
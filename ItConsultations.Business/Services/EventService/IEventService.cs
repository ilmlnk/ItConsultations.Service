using ItConsultations.Business.Dtos.AttachmentDtos;
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
    
    Task<IEnumerable<EventDto>> GetUserEventsAsync(GetUserEventsDto dto, string userId);
    
    Task<IEnumerable<EventDto>> GetUpcomingEventsAsync(int days);
    
    Task<EventDto> AddParticipantAsync(AddParticipantDto dto, string eventId, long userId);
    
    Task<bool> RemoveParticipantAsync(string eventId, long userId);
    
    Task<EventDto> UpdateParticipantStatusAsync(string eventId, long userId, ParticipantStatus status);
    
    Task<EventDto> AddAttachmentAsync(AddAttachmentDto dto, string eventId);
    
    Task<bool> RemoveAttachmentAsync(string eventId, long attachmentId);
    
    Task<EventDto> UpdateRecurrenceAsync(UpdateRecurrenceDto dto, string eventId);
    
    Task<IEnumerable<EventDto>> GetRecurringEventsAsync(string eventId);
    
    Task<EventDto> CancelEventAsync(CancelEventDto dto, string eventId);
    
    Task<EventDto> RescheduleEventAsync(RescheduleEventDto dto, string eventId);
    
    Task<bool> SendInvitationsAsync(string eventId);
    
    Task<bool> SendRemindersAsync(string eventId);
    
    Task<EventDto> SyncWithGoogleCalendarAsync(string eventId);
    
    Task<EventDto> CreateFromGoogleCalendarAsync(GoogleCalendarEventDto googleEvent, string creatorId);
    
    Task<bool> UpdateGoogleCalendarEventAsync(string eventId);
    
    Task<bool> DeleteGoogleCalendarEventAsync(string eventId);
    
    // Export functionality
    Task<string> ExportEventToICalendarAsync(string eventId);
    
    Task<string> ExportUserEventsToICalendarAsync(ExportEventsToICalendarDto dto, string userId);
    
    Task<string> ExportEventsToICalendarAsync(IEnumerable<long> eventIds);
    
    Task<string> GetGoogleCalendarImportUrlAsync(string userId);
    
    Task<string> GetGoogleCalendarImportUrlAsync(IEnumerable<long> eventIds);
    
    Task<bool> ExportEventToGoogleCalendarAsync(string eventId, string userAccessToken);
    
    Task<bool> ExportUserEventsToGoogleCalendarAsync(ExportEventsToGoogleCalendarDto dto, string userId, string userAccessToken);
} 
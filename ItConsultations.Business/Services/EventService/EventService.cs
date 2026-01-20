using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.AttachmentDtos;
using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Dtos.EventDtos;
using ItConsultations.Business.Entities.Attachments;
using ItConsultations.Business.Entities.Events;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.Services.GoogleCalendarService;
using ItConsultations.Business.SharedTypes.Enums.Event;
using ItConsultations.Utilities.Guards;
using Microsoft.Extensions.Logging;

namespace ItConsultations.Business.Services.EventService;

public class EventService : IEventService
{
    private readonly IRepository<Event, long> _eventRepository;
    private readonly IRepository<EventParticipant, long> _participantRepository;
    private readonly IRepository<Attachment, long> _attachmentRepository;
    private readonly IRepository<UserEntity, long> _userRepository;
    private readonly IGoogleCalendarService _googleCalendarService;
    private readonly ILogger<EventService> _logger;

    public EventService(
        IRepository<Event, long> eventRepository,
        IRepository<EventParticipant, long> participantRepository,
        IRepository<Attachment, long> attachmentRepository,
        IRepository<UserEntity, long> userRepository,
        IGoogleCalendarService googleCalendarService,
        ILogger<EventService> logger)
    {
        _eventRepository = eventRepository;
        _participantRepository = participantRepository;
        _attachmentRepository = attachmentRepository;
        _userRepository = userRepository;
        _googleCalendarService = googleCalendarService;
        _logger = logger;
    }

    public Task<EventDto> CreateAsync(CreateEventDto createDto, string creatorId)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> UpdateAsync(UpdateEventDto updateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(long eventId)
    {
        var eventEntity = await _eventRepository.GetAsync(eventId);

        if (!string.IsNullOrEmpty(eventEntity.GoogleCalendarEventId))
        {
            await _googleCalendarService.DeleteEventAsync(eventEntity.GoogleCalendarEventId);
        }

        await _eventRepository.DeleteAsync(eventEntity);

        return true;
    }

    public async Task<EventDto> GetByIdAsync(long eventId)
    {
        var eventEntity = await _eventRepository.GetAsync(eventId);
        return eventEntity != null ? MapperManager.Map<EventDto>(eventEntity) : null;
    }

    public async Task<EventDto> GetByConsIdAsync(string eventConsId)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventConsId).FirstOrDefault();
        return eventEntity != null ? MapperManager.Map<EventDto>(eventEntity) : null;
    }

    public Task<IEnumerable<EventDto>> SearchAsync(EventSearchDto searchDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<EventDto>> GetUserEventsAsync(GetUserEventsDto dto, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EventDto>> GetUpcomingEventsAsync(int days = 7)
    {
        var fromDate = DateTime.UtcNow;
        var toDate = fromDate.AddDays(days);

        var events = _eventRepository
            .Get(e => e.DeletedAt == null &&
            e.StartDateTime >= fromDate &&
            e.StartDateTime <= toDate).ToList();

        var result = new List<EventDto>();
        foreach (var eventEntity in events.OrderBy(e => e.StartDateTime))
        {
            result.Add(MapperManager.Map<EventDto>(eventEntity));
        }

        return result;
    }

    public Task<EventDto> AddParticipantAsync(AddParticipantDto dto, string eventId, long userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveParticipantAsync(string eventId, long userId)
    {
        var participant = _participantRepository.Get(p => p.EventConsId == eventId && p.UserId == userId);

        await _participantRepository.DeleteAsync(participant);
        return true;
    }

    public Task<EventDto> UpdateParticipantStatusAsync(string eventId, long userId, ParticipantStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> AddAttachmentAsync(AddAttachmentDto dto, string eventId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveAttachmentAsync(string eventId, long attachmentId)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> UpdateRecurrenceAsync(UpdateRecurrenceDto dto, string eventId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EventDto>> GetRecurringEventsAsync(string eventId)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> CancelEventAsync(CancelEventDto dto, string eventId)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> RescheduleEventAsync(RescheduleEventDto dto, string eventId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SendInvitationsAsync(string eventId)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();

        // Send invitations to participants
        var participants = _participantRepository.Get(p => p.EventConsId == eventId).ToList();

        foreach (var participant in participants)
        {
            // TODO: Implement email sending logic
            _logger.LogInformation("Sending invitation to {Email} for event {EventId}", participant.User.Email, eventId);
        }

        return true;
    }

    public async Task<bool> SendRemindersAsync(string eventId)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();

        // Send reminders to participants
        var participants = _participantRepository.Get(p => p.EventConsId == eventId && p.SendReminders).ToList();

        foreach (var participant in participants)
        {
            // TODO: Implement reminder sending logic
        }

        return true;
    }

    public async Task<EventDto> SyncWithGoogleCalendarAsync(string eventId)
    {
        var eventEntity = _eventRepository
            .Get(e => e.EventConsId == eventId)
            .FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        if (string.IsNullOrEmpty(eventEntity.GoogleCalendarEventId))
        {
            // Create new event in Google Calendar
            var googleEventId = await _googleCalendarService.CreateEventAsync(eventEntity);
            eventEntity.GoogleCalendarEventId = googleEventId;
            eventEntity.LastGoogleSync = DateTime.UtcNow;
        }
        else
        {
            // Update existing event in Google Calendar
            await _googleCalendarService.UpdateEventAsync(eventEntity);
            eventEntity.LastGoogleSync = DateTime.UtcNow;
        }

        await _eventRepository.UpdateAsync(eventEntity);

        return MapperManager.Map<EventDto>(eventEntity);
    }

    public Task<EventDto> CreateFromGoogleCalendarAsync(GoogleCalendarEventDto googleEvent, string creatorId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateGoogleCalendarEventAsync(string eventId)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        if (eventEntity == null || string.IsNullOrEmpty(eventEntity.GoogleCalendarEventId))
        {
            return false;
        }

        try
        {
            await _googleCalendarService.UpdateEventAsync(eventEntity);
            eventEntity.LastGoogleSync = DateTime.UtcNow;
            await _eventRepository.UpdateAsync(eventEntity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Google Calendar event {EventId}", eventId);
            return false;
        }
    }

    public async Task<bool> DeleteGoogleCalendarEventAsync(string eventId)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();

        if (eventEntity == null || string.IsNullOrEmpty(eventEntity.GoogleCalendarEventId))
        {
            return false;
        }

        try
        {
            await _googleCalendarService.DeleteEventAsync(eventEntity.GoogleCalendarEventId);
            eventEntity.GoogleCalendarEventId = null;
            eventEntity.LastGoogleSync = DateTime.UtcNow;
            await _eventRepository.UpdateAsync(eventEntity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Google Calendar event {EventId}", eventId);
            return false;
        }
    }

    // Export functionality
    public async Task<string> ExportEventToICalendarAsync(string eventId)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        Guard.NotNull(eventEntity, "Event not found");

        return await _googleCalendarService.ExportEventToICalendarAsync(eventEntity);
    }

    public Task<string> ExportUserEventsToICalendarAsync(ExportEventsToICalendarDto dto, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> ExportEventsToICalendarAsync(IEnumerable<long> eventIds)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetGoogleCalendarImportUrlAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetGoogleCalendarImportUrlAsync(IEnumerable<long> eventIds)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExportEventToGoogleCalendarAsync(string eventId, string userAccessToken)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        return await _googleCalendarService.ExportEventAsync(eventEntity, userAccessToken);
    }

    public Task<bool> ExportUserEventsToGoogleCalendarAsync(ExportEventsToGoogleCalendarDto dto, string userId, string userAccessToken)
    {
        throw new NotImplementedException();
    }
}
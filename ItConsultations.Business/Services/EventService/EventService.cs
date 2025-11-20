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

    public async Task<EventDto> CreateAsync(CreateEventDto createDto, string creatorConsId)
    {
        /*Guard.NotNull(createDto);
        Guard.NotNullOrEmpty(createDto.Title);*/

        var creator = _userRepository.Get(x => x.ConsId == creatorConsId).FirstOrDefault();
        //Guard.NotNull(creator, "Creator not found");

        var dto = MapperManager.Map<EventDto>(createDto);
        dto.CreatedAt = DateTime.UtcNow;
        dto.UpdatedAt = DateTime.UtcNow;

        var eventEntity = MapperManager.Map<Event>(dto);

        await AddParticipantsAsync();
        await AddAttachmentsAsync();

        // Sync with Google Calendar if needed
        if (ShouldSyncWithGoogleCalendar(eventEntity))
        {
            await SyncWithGoogleCalendarAsync(eventEntity.EventConsId);
        }

        await _eventRepository.CreateAsync(eventEntity);

        return dto;
    }

    /*public async Task<EventDto> UpdateAsync(UpdateEventDto updateDto, string eventConsId)
    {
        /*Guard.NotNull(updateDto);
        Guard.NotNullOrEmpty(updateDto.Title);

        var originalEvent = _eventRepository
            .Get(x => x.EventConsId == eventConsId)
            .FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        var dto = MapperManager.Map<EventDto>(originalEvent);
        dto.UpdatedAt = DateTime.UtcNow;
        var eventEntity = MapperManager.Map<Event>(dto);

        await _eventRepository.UpdateAsync(eventEntity);

        // Update participants
        var currentParticipants = _participantRepository.Get(p => p.EventConsId == eventEntity.EventConsId).ToList();
        var currentUserIds = currentParticipants.Select(p => p.UserId).ToList();
        var newUserIds = updateDto.ParticipantUserIds;

        // Remove participants not in new list
        var participantsToRemove = currentParticipants.Where(p => !newUserIds.Contains(p.UserId));
        foreach (var participant in participantsToRemove)
        {
            await _participantRepository.DeleteAsync(participant);
        }

        // Add new participants
        var participantsToAdd = newUserIds.Where(id => !currentUserIds.Contains(id));
        foreach (var userId in participantsToAdd)
        {
            await AddParticipantAsync(eventEntity.EventConsId, userId);
        }

        // Update attachments
        if (updateDto.AttachmentIds != null)
        {
            var currentAttachments = _attachmentRepository.Get(a => a.EventId == eventEntity.EventConsId).ToList();
            var currentAttachmentIds = currentAttachments.Select(a => a.AttachmentId).ToList();
            var newAttachmentIds = updateDto.AttachmentIds;

            // Remove attachments not in new list
            var attachmentsToRemove = currentAttachments.Where(a => !newAttachmentIds.Contains(a.AttachmentId));
            foreach (var attachment in attachmentsToRemove)
            {
                await _attachmentRepository.DeleteAsync(attachment);
            }

            // Add new attachments
            var attachmentsToAdd = newAttachmentIds.Where(id => !currentAttachmentIds.Contains(id));
            foreach (var attachmentId in attachmentsToAdd)
            {
                await AddAttachmentAsync(eventEntity.EventConsId, attachmentId);
            }
        }

        // Sync with Google Calendar if needed
        if (ShouldSyncWithGoogleCalendar(eventEntity))
        {
            try
            {
                await SyncWithGoogleCalendarAsync(eventEntity.EventConsId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to sync event {EventId} with Google Calendar", eventEntity.Id);
            }
        }

        return await MapToDtoAsync(eventEntity);
    }*/

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

    /*public async Task<IEnumerable<EventDto>> SearchAsync(EventSearchDto searchDto)
    {
        //Guard.NotNull(searchDto);

        var query = _eventRepository.Get(e => e.DeletedAt == null);

        // Apply filters
        if (!string.IsNullOrEmpty(searchDto.Title))
        {
            query = query.Where(e => e.Title.Contains(searchDto.Title));
        }

        if (!string.IsNullOrEmpty(searchDto.Description))
        {
            query = query.Where(e => e.Description != null && e.Description.Contains(searchDto.Description));
        }

        if (searchDto.BeginDateFrom.HasValue)
        {
            query = query.Where(e => e.BeginDateTime >= searchDto.BeginDateFrom.Value);
        }

        if (searchDto.BeginDateTo.HasValue)
        {
            query = query.Where(e => e.BeginDateTime <= searchDto.BeginDateTo.Value);
        }

        if (searchDto.CreatorId.HasValue)
        {
            query = query.Where(e => e.Creator.Id == searchDto.CreatorId.Value);
        }

        if (searchDto.Status.HasValue)
        {
            query = query.Where(e => e.Status == searchDto.Status.Value);
        }

        if (searchDto.Visibility.HasValue)
        {
            query = query.Where(e => e.Visibility == searchDto.Visibility.Value);
        }

        if (searchDto.RecurrenceType.HasValue)
        {
            query = query.Where(e => e.RecurrenceType == searchDto.RecurrenceType.Value);
        }

        if (searchDto.IsAllDay.HasValue)
        {
            query = query.Where(e => e.IsAllDay == searchDto.IsAllDay.Value);
        }

        if (!string.IsNullOrEmpty(searchDto.Location))
        {
            query = query.Where(e => e.Location != null && e.Location.Contains(searchDto.Location));
        }

        if (searchDto.HasMeetingUrl.HasValue)
        {
            if (searchDto.HasMeetingUrl.Value)
            {
                query = query.Where(e => !string.IsNullOrEmpty(e.MeetingUrl));
            }
            else
            {
                query = query.Where(e => string.IsNullOrEmpty(e.MeetingUrl));
            }
        }

        if (!string.IsNullOrEmpty(searchDto.MeetingProvider))
        {
            query = query.Where(e => e.MeetingProvider == searchDto.MeetingProvider);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(searchDto.SortBy))
        {
            query = searchDto.SortBy.ToLower() switch
            {
                "title" => searchDto.SortDirection == "desc"
                    ? query.OrderByDescending(e => e.Title)
                    : query.OrderBy(e => e.Title),
                "begindatetime" => searchDto.SortDirection == "desc"
                    ? query.OrderByDescending(e => e.BeginDateTime)
                    : query.OrderBy(e => e.BeginDateTime),
                "createdat" => searchDto.SortDirection == "desc"
                    ? query.OrderByDescending(e => e.CreatedAt)
                    : query.OrderBy(e => e.CreatedAt),
                _ => query.OrderBy(e => e.BeginDateTime)
            };
        }
        else
        {
            query = query.OrderBy(e => e.BeginDateTime);
        }

        // Apply pagination
        var skip = (searchDto.PageNumber - 1) * searchDto.PageSize;
        var events = query.Skip(skip).Take(searchDto.PageSize).ToList();

        var result = new List<EventDto>();
        foreach (var eventEntity in events)
        {
            result.Add(MapperManager.Map<EventDto>(eventEntity));
        }

        return result;
    }*/

    public async Task<IEnumerable<EventDto>> GetUserEventsAsync(long userId, DateTime fromDate, DateTime toDate)
    {
        var query = _participantRepository
            .Get(p => p.UserId == userId && p.Event.DeletedAt == null);

        query = query.Where(p => p.Event.BeginDateTime >= fromDate);

        query = query.Where(p => p.Event.BeginDateTime <= toDate);

        var participants = query.ToList();
        var events = participants.Select(p => p.Event).OrderBy(e => e.BeginDateTime);

        var result = new List<EventDto>();
        foreach (var eventEntity in events)
        {
            result.Add(MapperManager.Map<EventDto>(eventEntity));
        }

        return result;
    }

    public async Task<IEnumerable<EventDto>> GetUpcomingEventsAsync(int days = 7)
    {
        var fromDate = DateTime.UtcNow;
        var toDate = fromDate.AddDays(days);

        var events = _eventRepository
            .Get(e => e.DeletedAt == null &&
            e.BeginDateTime >= fromDate &&
            e.BeginDateTime <= toDate).ToList();

        var result = new List<EventDto>();
        foreach (var eventEntity in events.OrderBy(e => e.BeginDateTime))
        {
            result.Add(MapperManager.Map<EventDto>(eventEntity));
        }

        return result;
    }

    public async Task<EventDto> AddParticipantAsync(AddParticipantDto dto, string eventId, long userId, ParticipantRole role = ParticipantRole.Attendee)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        var user = await _userRepository.GetAsync(userId);
        //Guard.NotNull(user, "User not found");

        var existingParticipant = _participantRepository.Get(p => p.EventConsId == eventId && p.UserId == userId).FirstOrDefault();

        var participant = MapperManager.Map<EventParticipant>(dto);

        await _participantRepository.CreateAsync(participant);

        return MapperManager.Map<EventDto>(eventEntity);
    }

    public async Task<bool> RemoveParticipantAsync(string eventId, long userId)
    {
        var participant = _participantRepository.Get(p => p.EventConsId == eventId && p.UserId == userId);
        if (participant == null)
        {
            return false;
        }

        await _participantRepository.DeleteAsync(participant);
        return true;
    }

    public async Task<EventDto> UpdateParticipantStatusAsync(string eventId, long userId, ParticipantStatus status, string? comment = null)
    {
        var participant = _participantRepository
            .Get(p => p.EventConsId == eventId && p.UserId == userId)
            .FirstOrDefault();
        //Guard.NotNull(participant, "Participant not found");

        participant.Status = status;
        participant.ResponseDate = DateTime.UtcNow;
        participant.ResponseComment = comment;
        participant.UpdatedAt = DateTime.UtcNow;

        await _participantRepository.UpdateAsync(participant);

        var eventEntity = _eventRepository
            .Get(e => e.EventConsId == eventId)
            .FirstOrDefault();
        return MapperManager.Map<EventDto>(eventEntity);
    }

    /*public async Task<EventDto> AddAttachmentAsync(AddAttachmentDto attachmentDto, string eventId)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        var existingAttachment = _attachmentRepository
            .Get(a => a.EventId == eventId && a.AttachmentId == attachmentDto.AttachmentId)
            .FirstOrDefault();

        var eventAttachment = MapperManager.Map<Attachment>(attachmentDto);

        await _attachmentRepository.CreateAsync(eventAttachment);

        return MapperManager.Map<EventDto>(eventEntity);
    }

    public async Task<bool> RemoveAttachmentAsync(string eventId, long attachmentId)
    {
        var attachment = _attachmentRepository.Get(a => a.EventId == eventId && a.AttachmentId == attachmentId).FirstOrDefault();

        if (attachment == null)
        {
            return false;
        }

        await _attachmentRepository.DeleteAsync(attachment);
        return true;
    }*/

    /*public async Task<EventDto> UpdateRecurrenceAsync(string eventId, RecurrenceType recurrenceType, int? interval = null, DateTime? endDate = null)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        Guard.NotNull(eventEntity, "Event not found");

        eventEntity.RecurrenceType = recurrenceType;
        eventEntity.RecurrenceInterval = interval ?? 1;
        eventEntity.RecurrenceEndDate = endDate;
        eventEntity.UpdatedAt = DateTime.UtcNow;

        await _eventRepository.UpdateAsync(eventEntity);

        return await MapToDtoAsync(eventEntity);
    }*/

    public async Task<IEnumerable<EventDto>> GetRecurringEventsAsync(string eventId)
    {
        var originEvent = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        //Guard.NotNull(baseEvent, "Event not found");

        if (originEvent.RecurrenceType == RecurrenceType.None)
        {
            return new List<EventDto> { MapperManager.Map<EventDto>(originEvent) };
        }

        var baseEvent = MapperManager.Map<EventDto>(originEvent);

        // Generate recurring events based on recurrence rules
        var events = new List<EventDto>();
        var currentDate = baseEvent.BeginDateTime;
        var endDate = baseEvent.RecurrenceEndDate ?? DateTime.UtcNow.AddYears(1);

        while (currentDate <= endDate)
        {
            events.Add(baseEvent);

            // Calculate next occurrence
            currentDate = CalculateNextOccurrence(currentDate, baseEvent.RecurrenceType, baseEvent.RecurrenceInterval, baseEvent.RecurrenceDayOfWeek, baseEvent.RecurrenceDayOfMonth);
        }

        return events;
    }

    public async Task<EventDto> CancelEventAsync(string eventId, string? reason = null)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        eventEntity.Status = EventStatus.Cancelled;
        eventEntity.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(reason))
        {
            eventEntity.Description = $"{eventEntity.Description}\n\nCancelled: {reason}";
        }

        await _eventRepository.UpdateAsync(eventEntity);

        // Update Google Calendar if synced
        if (!string.IsNullOrEmpty(eventEntity.GoogleCalendarEventId))
        {
            await _googleCalendarService.UpdateEventAsync(eventEntity);
        }

        return MapperManager.Map<EventDto>(eventEntity);
    }

    public async Task<EventDto> RescheduleEventAsync(string eventId, DateTime newBeginDateTime, DateTime newEndDateTime)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        eventEntity.BeginDateTime = newBeginDateTime;
        eventEntity.EndDateTime = newEndDateTime;
        eventEntity.UpdatedAt = DateTime.UtcNow;

        await _eventRepository.UpdateAsync(eventEntity);

        // Update Google Calendar if synced
        if (!string.IsNullOrEmpty(eventEntity.GoogleCalendarEventId))
        {
            await _googleCalendarService.UpdateEventAsync(eventEntity);
        }

        return MapperManager.Map<EventDto>(eventEntity);
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

    /*public async Task<EventDto> CreateFromGoogleCalendarAsync(GoogleCalendarEventDto googleEvent, long creatorId)
    {
        //Guard.NotNull(googleEvent);

        var creator = await _userRepository.GetAsync(creatorId);
        //Guard.NotNull(creator, "Creator not found");

        var eventEntity = MapperManager.Map<EventDto>(googleEvent);

        await _eventRepository.CreateAsync(eventEntity);

        // Add attendees as participants
        if (googleEvent.Attendees?.Any() == true)
        {
            foreach (var attendee in googleEvent.Attendees)
            {
                var user = _userRepository.Get(u => u.Email == attendee.Email).FirstOrDefault();
                if (user != null)
                {
                    await AddParticipantAsync(eventEntity.EventConsId, user.Id,
                        attendee.Optional == true ? ParticipantRole.Optional : ParticipantRole.Attendee);
                }
            }
        }

        return eventEntity;
    }*/

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

    public async Task<string> ExportUserEventsToICalendarAsync(long userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var participants = _participantRepository.Get(p => p.UserId == userId).ToList();
        var eventIds = participants.Select(p => p.EventConsId).ToList();

        var events = _eventRepository.Get(e => eventIds.Contains(e.EventConsId) && e.DeletedAt == null).ToList();

        // Apply date filters
        if (fromDate.HasValue)
        {
            events = events.Where(e => e.BeginDateTime >= fromDate.Value).ToList();
        }

        if (toDate.HasValue)
        {
            events = events.Where(e => e.BeginDateTime <= toDate.Value).ToList();
        }

        return await _googleCalendarService.ExportEventsToICalendarAsync(events);
    }

    public async Task<string> ExportEventsToICalendarAsync(IEnumerable<string> eventIds)
    {
        var events = _eventRepository.Get(e => eventIds.Contains(e.EventConsId) && e.DeletedAt == null).ToList();
        return await _googleCalendarService.ExportEventsToICalendarAsync(events);
    }

    public async Task<string> GetGoogleCalendarImportUrlAsync(long userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var participants = _participantRepository.Get(p => p.UserId == userId).ToList();
        var eventIds = participants.Select(p => p.EventConsId).ToList();

        var events = _eventRepository.Get(e => eventIds.Contains(e.EventConsId) && e.DeletedAt == null).ToList();

        // Apply date filters
        if (fromDate.HasValue)
        {
            events = events.Where(e => e.BeginDateTime >= fromDate.Value).ToList();
        }

        if (toDate.HasValue)
        {
            events = events.Where(e => e.BeginDateTime <= toDate.Value).ToList();
        }

        return await _googleCalendarService.GetGoogleCalendarImportUrlAsync(events);
    }

    public async Task<string> GetGoogleCalendarImportUrlAsync(IEnumerable<string> eventIds)
    {
        var events = _eventRepository.Get(e => eventIds.Contains(e.EventConsId) && e.DeletedAt == null).ToList();
        return await _googleCalendarService.GetGoogleCalendarImportUrlAsync(events);
    }

    public async Task<bool> ExportEventToGoogleCalendarAsync(string eventId, string userAccessToken)
    {
        var eventEntity = _eventRepository.Get(e => e.EventConsId == eventId).FirstOrDefault();
        //Guard.NotNull(eventEntity, "Event not found");

        return await _googleCalendarService.ExportEventAsync(eventEntity, userAccessToken);
    }

    public async Task<bool> ExportUserEventsToGoogleCalendarAsync(string userId, string userAccessToken, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var participants = _participantRepository.Get(p => p.ParticipantConsId == userId).ToList();
        var eventIds = participants.Select(p => p.EventConsId).ToList();

        var events = _eventRepository.Get(e => eventIds.Contains(e.EventConsId) && e.DeletedAt == null).ToList();

        // Apply date filters
        if (fromDate.HasValue)
        {
            events = events.Where(e => e.BeginDateTime >= fromDate.Value).ToList();
        }

        if (toDate.HasValue)
        {
            events = events.Where(e => e.BeginDateTime <= toDate.Value).ToList();
        }

        return await _googleCalendarService.ExportEventsToGoogleCalendarAsync(events, userAccessToken);
    }

    public Task<IEnumerable<EventDto>> GetUserEventsAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> CreateFromGoogleCalendarAsync(GoogleCalendarEventDto googleEvent, string creatorId)
    {
        throw new NotImplementedException();
    }

    public Task<string> ExportUserEventsToICalendarAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> ExportEventsToICalendarAsync(IEnumerable<long> eventIds)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetGoogleCalendarImportUrlAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetGoogleCalendarImportUrlAsync(IEnumerable<long> eventIds)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> AddParticipantAsync(string eventId, long userId, AddParticipantDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> UpdateAsync(UpdateEventDto updateDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<EventDto>> SearchAsync(EventSearchDto searchDto)
    {
        throw new NotImplementedException();
    }

    private async Task AddParticipantsAsync()
    {

    }

    private async Task AddAttachmentsAsync()
    {

    }

    private bool ShouldSyncWithGoogleCalendar(Event eventEntity)
    {
        return eventEntity.Status == EventStatus.Scheduled && eventEntity.Visibility != EventVisibility.Private;
    }

    private DateTime CalculateNextOccurrence(DateTime currentDate, RecurrenceType recurrenceType, int? interval, DayOfWeek? dayOfWeek, int? dayOfMonth)
    {
        return recurrenceType switch
        {
            RecurrenceType.Daily => currentDate.AddDays(interval ?? 1),
            RecurrenceType.Weekly => currentDate.AddDays(7 * (interval ?? 1)),
            RecurrenceType.Monthly => currentDate.AddMonths(interval ?? 1),
            RecurrenceType.Yearly => currentDate.AddYears(interval ?? 1),
            _ => currentDate.AddDays(1)
        };
    }

    public Task<IEnumerable<EventDto>> GetUserEventsAsync(GetUserEventsDto dto, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> AddParticipantAsync(AddParticipantDto dto, string eventId, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> UpdateParticipantStatusAsync(string eventId, long userId, ParticipantStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> AddAttachmentAsync(AddAttachmentDto dto, string eventId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAttachmentAsync(string eventId, long attachmentId)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> UpdateRecurrenceAsync(UpdateRecurrenceDto dto, string eventId)
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

    public Task<string> ExportUserEventsToICalendarAsync(ExportEventsToICalendarDto dto, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetGoogleCalendarImportUrlAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExportUserEventsToGoogleCalendarAsync(ExportEventsToGoogleCalendarDto dto, string userId, string userAccessToken)
    {
        throw new NotImplementedException();
    }
}
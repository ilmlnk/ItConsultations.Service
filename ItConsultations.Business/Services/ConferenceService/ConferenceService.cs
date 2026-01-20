using ItConsultations.Business.AutoMapperConfiguration;
using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Dtos.UserDtos;
using ItConsultations.Business.Entities;
using ItConsultations.Business.Entities.Conferences;
using ItConsultations.Business.SharedTypes.Enums.Conference;

namespace ItConsultations.Business.Services.ConferenceService;

public class ConferenceService : IConferenceService
{
    private readonly IRepository<Conference, long> _conferenceRepository;
    private readonly IRepository<ConferenceParticipant, long> _participantRepository;
    private readonly IRepository<ConferenceRecording, long> _recordingRepository;

    public ConferenceService(
        IRepository<Conference, long> conferenceRepository,
        IRepository<ConferenceParticipant, long> participantRepository,
        IRepository<ConferenceRecording, long> recordingRepository)
    {
        _conferenceRepository = conferenceRepository;
        _participantRepository = participantRepository;
        _recordingRepository = recordingRepository;
    }

    public async Task<ConferenceDto> CreateConferenceAsync(CreateConferenceDto dto)
    {
        var conference = MapperManager.Map<Conference>(dto);
        conference.ConferenceUrl = GenerateConferenceUrl();
        conference.Status = ConferenceStatus.Scheduled;

        conference = await _conferenceRepository.CreateAsync(conference);

        if (!dto.ParticipantUserIds.Any())
        {
            return MapperManager.Map<ConferenceDto>(conference);
        }
        
        foreach (var participant in dto.ParticipantUserIds.Select(userId => 
            new ConferenceParticipant {
                ConferenceConsId = conference.ConferenceConsId,
                UserId = userId,
                Role = ConferenceParticipantRole.Guest
            }))
        {
            await _participantRepository.CreateAsync(participant);
        }

        return MapperManager.Map<ConferenceDto>(conference);
    }

    public async Task<ConferenceDto> UpdateConferenceAsync(long id, UpdateConferenceDto dto)
    {
        var conference = await _conferenceRepository.GetAsync(id);

        MapperManager.Map(dto, conference);
        conference.UpdatedAt = DateTime.UtcNow;

        await _conferenceRepository.UpdateAsync(conference);
        return MapperManager.Map<ConferenceDto>(conference);
    }

    public async Task<bool> DeleteConferenceAsync(long id)
    {
        var conference = await _conferenceRepository.GetAsync(id);

        await _conferenceRepository.DeleteAsync(conference);
        return true;
    }

    public async Task<ConferenceDto> GetConferenceAsync(long id)
    {
        var conference = await _conferenceRepository.GetAsync(id);
        return MapperManager.Map<ConferenceDto>(conference);
    }

    public async Task<ConferenceDto?> GetConferenceAsync(string id)
    {
        var conference = _conferenceRepository.Get(x => x.ConferenceConsId == id).FirstOrDefault();
        return conference != null ? MapperManager.Map<ConferenceDto>(conference) : null;
    }

    public Task<IEnumerable<ConferenceDto>> GetUserConferencesAsync(long userId)
    {
        throw new NotImplementedException();
        /*var conferences = _conferenceRepository.Get(c => c.Organizer.UserId == userId || c.Participants.Any(p => p.UserId == userId));
        return Task.FromResult(MapperManager.Map<IEnumerable<ConferenceDto>>(conferences));*/
    }

    public Task<ConferenceDto[]> SearchConferencesAsync(ConferenceSearchDto searchDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ConferenceDto> JoinConferenceAsync(string conferenceId, long userId)
    {
        var conference = _conferenceRepository.Get(c => c.ConferenceConsId == conferenceId).FirstOrDefault();

        if (conference == null)
        {
            return null;
        }

        var existingParticipant = conference.Participants.FirstOrDefault(p => p.UserId == userId);
        if (existingParticipant == null)
        {
            var participant = new ConferenceParticipant
            {
                ConferenceConsId = conferenceId,
                UserId = userId,
                Role = ConferenceParticipantRole.Guest,
                JoinedAt = DateTime.UtcNow
            };

            await _participantRepository.CreateAsync(participant);
        }
        else if (existingParticipant.LeftAt.HasValue)
        {
            existingParticipant.LeftAt = null;
            existingParticipant.JoinedAt = DateTime.UtcNow;
            await _participantRepository.UpdateAsync(existingParticipant);
        }

        return MapperManager.Map<ConferenceDto>(conference);
    }

    public async Task<bool> ActivateRecordingAsync(string conferenceId, bool enableRecording, bool enableChatRecording)
    {
        var conference = _conferenceRepository.Get(c => c.ConferenceConsId == conferenceId).FirstOrDefault();

        if (conference == null)
        {
            return false;
        }

        conference.IsRecordingEnabled = enableRecording;
        conference.IsChatRecordingEnabled = enableChatRecording;
        conference.UpdatedAt = DateTime.UtcNow;

        await _conferenceRepository.UpdateAsync(conference);
        return true;
    }

    public async Task<ConferenceRecordingDto> UploadRecordingAsync(string conferenceId, byte[] file, string fileName)
    {
        var conference = _conferenceRepository.Get(c => c.ConferenceConsId == conferenceId).FirstOrDefault();

        if (conference == null)
        {
            return null;
        }

        var recordingUrl = await SaveFileAsync(file, fileName, "recordings");

        var recording = new ConferenceRecording
        {
            ConferenceRecordingConsId = conferenceId,
            RecordingUrl = recordingUrl,
            StartedAt = DateTime.UtcNow,
            IsActive = false
        };

        recording = await _recordingRepository.CreateAsync(recording);
        return MapperManager.Map<ConferenceRecordingDto>(recording);
    }

    public async Task<ConferenceRecordingDto> UploadChatLogAsync(string conferenceId, byte[] file, string fileName)
    {
        var conference = _conferenceRepository.Get(c => c.ConferenceConsId == conferenceId);

        if (conference == null)
        {
            return null;
        }

        var chatLogUrl = await SaveFileAsync(file, fileName, "chatlogs");

        var recording = new ConferenceRecording
        {
            ConferenceRecordingConsId = conferenceId,
            ChatLogUrl = chatLogUrl,
            StartedAt = DateTime.UtcNow,
            IsActive = false
        };

        recording = await _recordingRepository.CreateAsync(recording);
        return MapperManager.Map<ConferenceRecordingDto>(recording);
    }

    public async Task<byte[]> DownloadRecordingAsync(string recordingId)
    {
        var recording = _recordingRepository.Get(r => r.ConferenceRecordingConsId == recordingId).FirstOrDefault();

        if (recording == null || string.IsNullOrEmpty(recording.RecordingUrl))
        {
            return null;
        }

        // TODO: Implement file loading logic
        return await LoadFileAsync(recording.RecordingUrl.Trim());
    }

    public async Task<byte[]> DownloadRecordingAsync(int recordingId)
    {
        var recording = await _recordingRepository.GetAsync(recordingId);

        if (recording == null || string.IsNullOrEmpty(recording.RecordingUrl))
        {
            return null;
        }

        return await LoadFileAsync(recording.RecordingUrl.Trim());
    }

    public async Task<byte[]> DownloadChatLogAsync(int recordingId)
    {
        var recording = await _recordingRepository.GetAsync(recordingId);

        if (recording == null || string.IsNullOrEmpty(recording.RecordingUrl))
        {
            return null;
        }

        return await LoadFileAsync(recording.ChatLogUrl);
    }

    public async Task<byte[]> DownloadChatLogAsync(string recordingId)
    {
        var recording = _recordingRepository.Get(r => r.ConferenceRecordingConsId == recordingId).FirstOrDefault();

        if (recording == null || string.IsNullOrEmpty(recording.ChatLogUrl))
        {
            return null;
        }

        return await LoadFileAsync(recording.ChatLogUrl);
    }

    public Task<ConferenceDto[]> GetUpcomingConferencesAsync(int days)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResumeConferenceAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResumeConferenceAsync(string consId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ConferenceDto>> GetUserConferencesAsync(long userId, DateTime? fromDate, DateTime? toDate)
    {
        throw new NotImplementedException();
    }

    public Task<ConferenceDto> JoinConferenceAsync(JoinConferenceDto joinDto, string id)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> DownloadRecordingAsync(long recordingId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> DownloadChatLogAsync(long recordingId)
    {
        throw new NotImplementedException();
    }

    public Task<ConferenceDto?> DeleteRecordingAsync(long recordingId)
    {
        throw new NotImplementedException();
    }

    public Task<ConferenceDto?> DeleteRecordingAsync(string recordingId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LeaveConferenceAsync(string conferenceId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ConferenceDto> StartConferenceAsync(string conferenceId)
    {
        throw new NotImplementedException();
    }

    public Task<ConferenceDto> PauseConferenceAsync(string conferenceId)
    {
        throw new NotImplementedException();
    }

    public Task<ConferenceDto> EndConferenceAsync(string conferenceId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> AddParticipantAsync(string conferenceId, AddParticipantDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> RemoveParticipantAsync(string conferenceId, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateParticipantRoleAsync(UpdateParticipantRoleDto dto, string conferenceId, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<object> GetConferenceStatisticsAsync(string conferenceId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ConferenceDto>> GetUserConferences(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ConferenceDto>> GetUserConferencesAsync(string userId, DateTime? fromDate, DateTime? toDate)
    {
        throw new NotImplementedException();
    }

    private string GenerateConferenceUrl()
    {
        var guid = Guid.NewGuid().ToString("N");
        return $"/conference/{guid}";
    }

    private async Task<string> SaveFileAsync(byte[] file, string fileName, string folder)
    {
        // TODO: Implement file saving logic
        await Task.Delay(100);
        return $"/files/{folder}/{Guid.NewGuid()}_{fileName}";
    }

    private async Task<byte[]> LoadFileAsync(string fileUrl)
    {
        // TODO: Implement file loading logic
        await Task.Delay(100);
        return new byte[0];
    }
}
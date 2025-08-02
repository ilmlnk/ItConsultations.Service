using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Dtos.UserDtos;

namespace ItConsultations.Business.Services.ConferenceService;

public interface IConferenceService
{
    Task<ConferenceDto> CreateConferenceAsync(CreateConferenceDto dto);

    Task<ConferenceDto> UpdateConferenceAsync(long id, UpdateConferenceDto dto);

    Task<bool> DeleteConferenceAsync(long id);

    Task<ConferenceDto> GetConferenceAsync(long id);

    Task<ConferenceDto> GetConferenceAsync(string consId);

    Task<IEnumerable<ConferenceDto>> GetUserConferencesAsync(long userId);

    Task<IEnumerable<ConferenceDto>> GetUserConferences(string userId);

    Task<IEnumerable<ConferenceDto>> GetUserConferencesAsync(long userId, DateTime? startDate, DateTime? endDate);
    
    Task<IEnumerable<ConferenceDto>> GetUserConferencesAsync(string userId, DateTime? fromDate, DateTime? toDate);

    Task<ConferenceDto> JoinConferenceAsync(string conferenceId, long userId);

    Task<ConferenceDto> JoinConferenceAsync(JoinConferenceDto joinDto, string id);

    Task<bool> ActivateRecordingAsync(string conferenceId, bool enableRecording, bool enableChatRecording);

    Task<ConferenceRecordingDto> UploadRecordingAsync(string conferenceId, byte[] file, string fileName);

    Task<ConferenceRecordingDto> UploadChatLogAsync(string conferenceId, byte[] file, string fileName);

    Task<byte[]> DownloadRecordingAsync(long recordingId);

    Task<byte[]> DownloadRecordingAsync(string recordingId);

    Task<byte[]> DownloadChatLogAsync(long recordingId);

    Task<byte[]> DownloadChatLogAsync(string recordingId);

    Task<bool> LeaveConferenceAsync(string conferenceId, string userId);

    Task<ConferenceDto> StartConferenceAsync(string conferenceId); 

    Task<ConferenceDto> PauseConferenceAsync(string conferenceId);

    Task<ConferenceDto> EndConferenceAsync(string conferenceId);

    Task<UserDto> AddParticipantAsync(string conferenceId, AddParticipantDto dto);

    Task<UserDto> RemoveParticipantAsync(string conferenceId, long userId);

    Task<UserDto> UpdateParticipantRoleAsync(UpdateParticipantRoleDto dto, string conferenceId, long userId);

    Task<object> GetConferenceStatisticsAsync(string conferenceId);

    Task<ConferenceDto> DeleteRecordingAsync(long recordingId);

    Task<ConferenceDto?> DeleteRecordingAsync(string recordingId);

    Task<ConferenceDto[]> SearchConferencesAsync(ConferenceSearchDto searchDto);

    Task<ConferenceDto[]> GetUpcomingConferencesAsync(int days);

    Task<bool> ResumeConferenceAsync(long id);

    Task<bool> ResumeConferenceAsync(string consId);
}
using AutoMapper;
using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Entities.Consultations;
using ItConsultations.Business.Entities.Articles;
using ItConsultations.Business.Dtos.NoteDtos;
using ItConsultations.Business.Dtos.ConferenceDtos.Conference;
using ItConsultations.Business.Entities.Students;
using ItConsultations.Business.Entities.Coaches;
using ItConsultations.Business.Entities.Conferences;
using ItConsultations.Business.Entities.Notes;
using ItConsultations.Business.Dtos.AuthDtos;
using ItConsultations.Business.Entities.Users;
using ItConsultations.Business.Entities.Events;
using ItConsultations.Business.Dtos.EventDtos;
using ItConsultations.Business.Dtos.UserDtos;


namespace ItConsultations.Business.AutoMapperConfiguration;

public class ConsultationsAutoMapperProfile : Profile
{
    public ConsultationsAutoMapperProfile()
    {
        CreateMappings();
    }

    private void CreateMappings()
    {
        // Coach mappings
        CreateMap<CreateCoachDto, Coach>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CoachConsId, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.Consultations, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Coach, CoachDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CoachConsId, opt => opt.MapFrom(src => src.CoachConsId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.ConsultationIds, opt => opt.MapFrom(src => src.Consultations != null ? src.Consultations.Select(c => c.Id) : new List<long>()))
            .ForMember(dest => dest.ReviewIds, opt => opt.MapFrom(src => src.Reviews != null ? src.Reviews.Select(r => r.Id) : new List<long>()))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.AverageRating));

        CreateMap<UpdateCoachDto, Coach>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CoachConsId, opt => opt.Ignore()) // Не изменяется при обновлении
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.Consultations, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // Consultation mappings
        CreateMap<Consultation, ConsultationDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl))
            .ForMember(dest => dest.Coach, opt => opt.MapFrom(src => src.Coach));

        // Student mappings
        CreateMap<CreateStudentDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StudentConsId, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PictureUrl, opt => opt.Ignore())
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.Consultation, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<UpdateStudentDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StudentConsId, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PictureUrl, opt => opt.Ignore())
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.Consultation, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentConsId, opt => opt.MapFrom(src => src.StudentConsId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl));

        // Article mappings
        CreateMap<CreateArticleDto, Article>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleConsId, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ArticleConsId, opt => opt.MapFrom(src => src.ArticleConsId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));

        // Note mappings
        CreateMap<CreateNoteDto, Note>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.ConsultationId, opt => opt.MapFrom(src => src.ConsultationId))
            .ForMember(dest => dest.CoachId, opt => opt.MapFrom(src => src.CoachId))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
            //.ForMember(dest => dest.ScheduledTime, opt => opt.Ignore())
            .ForMember(dest => dest.IsPinned, opt => opt.MapFrom(src => src.IsPinned))
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.LastViewedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Consultation, opt => opt.Ignore())
            .ForMember(dest => dest.Coach, opt => opt.Ignore())
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Author, opt => opt.Ignore());

        CreateMap<UpdateNoteDto, Note>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
            //.ForMember(dest => dest.ScheduledTime, opt => opt.Ignore())
            .ForMember(dest => dest.IsPinned, opt => opt.MapFrom(src => src.IsPinned))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ConsultationId, opt => opt.Ignore())
            .ForMember(dest => dest.CoachId, opt => opt.Ignore())
            .ForMember(dest => dest.StudentId, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.LastViewedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Consultation, opt => opt.Ignore())
            .ForMember(dest => dest.Coach, opt => opt.Ignore())
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Author, opt => opt.Ignore());

        CreateMap<Note, NoteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ConsultationId, opt => opt.MapFrom(src => src.ConsultationId))
            .ForMember(dest => dest.CoachId, opt => opt.MapFrom(src => src.CoachId))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            /*.ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => 
                src.Author != null ? $"{src.Author.FirstName} {src.Author.LastName}".Trim() : string.Empty))*/
            .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => 
                src.Author != null ? src.Author.Email : string.Empty))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
            //.ForMember(dest => dest.ScheduledTime, opt => opt.MapFrom(src => src.ScheduledTime))
            .ForMember(dest => dest.IsPinned, opt => opt.MapFrom(src => src.IsPinned))
            .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
            .ForMember(dest => dest.LastViewedAt, opt => opt.MapFrom(src => src.LastViewedAt))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        // Conference mappings
        CreateMap<CreateConferenceDto, Conference>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.Consultation, opt => opt.Ignore())
            .ForMember(dest => dest.IsRecordingEnabled, opt => opt.MapFrom(src => src.IsRecordingEnabled))
            .ForMember(dest => dest.IsChatRecordingEnabled, opt => opt.MapFrom(src => src.IsChatRecordingEnabled))
            .ForMember(dest => dest.ConferenceUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            .ForMember(dest => dest.Notes, opt => opt.Ignore())
            .ForMember(dest => dest.Recordings, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Organizer, opt => opt.Ignore())
            .ForMember(dest => dest.Consultation, opt => opt.Ignore());

        CreateMap<UpdateConferenceDto, Conference>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
            .ForMember(dest => dest.IsRecordingEnabled, opt => opt.MapFrom(src => src.IsRecordingEnabled))
            .ForMember(dest => dest.IsChatRecordingEnabled, opt => opt.MapFrom(src => src.IsChatRecordingEnabled))
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            .ForMember(dest => dest.Notes, opt => opt.Ignore())
            .ForMember(dest => dest.Recordings, opt => opt.Ignore())
            .ForMember(dest => dest.Organizer, opt => opt.Ignore())
            .ForMember(dest => dest.Consultation, opt => opt.Ignore())
            .ForMember(dest => dest.ConferenceUrl, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Conference, ConferenceDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
            //.ForMember(dest => dest.OrganizerId, opt => opt.MapFrom(src => src.OrganizerId))
            //.ForMember(dest => dest.OrganizerName, opt => opt.MapFrom(src => src.Organizer != null ? (src.Organizer.FirstName + " " + src.Organizer.LastName).Trim() : string.Empty))
            //.ForMember(dest => dest.ConsultationId, opt => opt.MapFrom(src => src.ConsultationId))
            //.ForMember(dest => dest.ConferenceUrl, opt => opt.MapFrom(src => src.ConferenceUrl))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.IsRecordingEnabled, opt => opt.MapFrom(src => src.IsRecordingEnabled))
            .ForMember(dest => dest.IsChatRecordingEnabled, opt => opt.MapFrom(src => src.IsChatRecordingEnabled))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            //.ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.Recordings, opt => opt.MapFrom(src => src.Recordings))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        // ConferenceParticipant mappings
        CreateMap<ConferenceParticipant, ConferenceParticipantDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.ConferenceId, opt => opt.MapFrom(src => src.ConferenceId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            //.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? (src.User.FirstName + " " + src.User.LastName).Trim() : string.Empty))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(src => src.JoinedAt))
            .ForMember(dest => dest.LeftAt, opt => opt.MapFrom(src => src.LeftAt));

        // ConferenceRecording mappings
        CreateMap<ConferenceRecording, ConferenceRecordingDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        //.ForMember(dest => dest.ConferenceId, opt => opt.MapFrom(src => src.ConferenceId))
        /*.ForMember(dest => dest.RecordingUrl, opt => opt.MapFrom(src => src.RecordingUrl))
        .ForMember(dest => dest.ChatLogUrl, opt => opt.MapFrom(src => src.ChatLogUrl))
        .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => src.StartedAt))
        .ForMember(dest => dest.EndedAt, opt => opt.MapFrom(src => src.EndedAt))
        .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));*/

        // *** FIREBASE AUTH ***
        CreateMap<RegisterDto, UserEntity>()
            .ForMember(dest => dest.FirebaseUid, opt => opt.MapFrom(src => src.FirebaseUid))
            .ForMember(dest => dest.ConsId, opt => opt.MapFrom(src => src.ConsId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PictureUrl))
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        CreateMap<UserEntity, UserDto>();
        CreateMap<UserInfoDto, UserEntity>()
            .ForMember(dest => dest.FirebaseUid, opt => opt.MapFrom(src => src.FirebaseUid))
            .ForMember(dest => dest.ConsId, opt => opt.MapFrom(src => src.ConsId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        
        CreateMap<UserEntity, UserInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ConsId, opt => opt.MapFrom(src => src.ConsId))
            .ForMember(dest => dest.FirebaseUid, opt => opt.MapFrom(src => src.FirebaseUid))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
            .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.GitHubUrl))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

        CreateMap<UserEntity, LoginResponseDto>();

        CreateMap<Event, CreateEventDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Conference, opt => opt.MapFrom(src => src.Conference))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.ReminderTime, opt => opt.MapFrom(src => src.ReminderTime))
            .ForMember(dest => dest.ReminderMinutes, opt => opt.MapFrom(src => src.ReminderMinutes))
            .ForMember(dest => dest.RecurrenceType, opt => opt.MapFrom(src => src.RecurrenceType))
            .ForMember(dest => dest.RecurrenceInterval, opt => opt.MapFrom(src => src.RecurrenceInterval))
            .ForMember(dest => dest.RecurrenceDayOfWeek, opt => opt.MapFrom(src => src.RecurrenceDayOfWeek))
            .ForMember(dest => dest.RecurrenceDayOfMonth, opt => opt.MapFrom(src => src.RecurrenceDayOfMonth))
            .ForMember(dest => dest.RecurrenceEndDate, opt => opt.MapFrom(src => src.RecurrenceEndDate))
            .ForMember(dest => dest.RecurrenceCount, opt => opt.MapFrom(src => src.RecurrenceCount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.IsAllDay, opt => opt.MapFrom(src => src.IsAllDay))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

        CreateMap<EventDto, CreateEventDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Conference, opt => opt.MapFrom(src => src.Conference))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.ReminderTime, opt => opt.MapFrom(src => src.ReminderTime))
            .ForMember(dest => dest.ReminderMinutes, opt => opt.MapFrom(src => src.ReminderMinutes))
            .ForMember(dest => dest.RecurrenceType, opt => opt.MapFrom(src => src.RecurrenceType))
            .ForMember(dest => dest.RecurrenceInterval, opt => opt.MapFrom(src => src.RecurrenceInterval))
            .ForMember(dest => dest.RecurrenceDayOfWeek, opt => opt.MapFrom(src => src.RecurrenceDayOfWeek))
            .ForMember(dest => dest.RecurrenceDayOfMonth, opt => opt.MapFrom(src => src.RecurrenceDayOfMonth))
            .ForMember(dest => dest.RecurrenceEndDate, opt => opt.MapFrom(src => src.RecurrenceEndDate))
            .ForMember(dest => dest.RecurrenceCount, opt => opt.MapFrom(src => src.RecurrenceCount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.IsAllDay, opt => opt.MapFrom(src => src.IsAllDay))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

        CreateMap<EventDto, Event>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Conference, opt => opt.MapFrom(src => src.Conference))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.ReminderTime, opt => opt.MapFrom(src => src.ReminderTime))
            .ForMember(dest => dest.ReminderMinutes, opt => opt.MapFrom(src => src.ReminderMinutes))
            .ForMember(dest => dest.RecurrenceType, opt => opt.MapFrom(src => src.RecurrenceType))
            .ForMember(dest => dest.RecurrenceInterval, opt => opt.MapFrom(src => src.RecurrenceInterval))
            .ForMember(dest => dest.RecurrenceDayOfWeek, opt => opt.MapFrom(src => src.RecurrenceDayOfWeek))
            .ForMember(dest => dest.RecurrenceDayOfMonth, opt => opt.MapFrom(src => src.RecurrenceDayOfMonth))
            .ForMember(dest => dest.RecurrenceEndDate, opt => opt.MapFrom(src => src.RecurrenceEndDate))
            .ForMember(dest => dest.RecurrenceCount, opt => opt.MapFrom(src => src.RecurrenceCount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.IsAllDay, opt => opt.MapFrom(src => src.IsAllDay))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

        CreateMap<UpdateEventDto, Event>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Conference, opt => opt.MapFrom(src => src.Conference))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.ReminderTime, opt => opt.MapFrom(src => src.ReminderTime))
            .ForMember(dest => dest.ReminderMinutes, opt => opt.MapFrom(src => src.ReminderMinutes))
            .ForMember(dest => dest.RecurrenceType, opt => opt.MapFrom(src => src.RecurrenceType))
            .ForMember(dest => dest.RecurrenceInterval, opt => opt.MapFrom(src => src.RecurrenceInterval))
            .ForMember(dest => dest.RecurrenceDayOfWeek, opt => opt.MapFrom(src => src.RecurrenceDayOfWeek))
            .ForMember(dest => dest.RecurrenceDayOfMonth, opt => opt.MapFrom(src => src.RecurrenceDayOfMonth))
            .ForMember(dest => dest.RecurrenceEndDate, opt => opt.MapFrom(src => src.RecurrenceEndDate))
            .ForMember(dest => dest.RecurrenceCount, opt => opt.MapFrom(src => src.RecurrenceCount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.IsAllDay, opt => opt.MapFrom(src => src.IsAllDay))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

        CreateMap<UpdateEventDto, EventDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Conference, opt => opt.MapFrom(src => src.Conference))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.ReminderTime, opt => opt.MapFrom(src => src.ReminderTime))
            .ForMember(dest => dest.ReminderMinutes, opt => opt.MapFrom(src => src.ReminderMinutes))
            .ForMember(dest => dest.RecurrenceType, opt => opt.MapFrom(src => src.RecurrenceType))
            .ForMember(dest => dest.RecurrenceInterval, opt => opt.MapFrom(src => src.RecurrenceInterval))
            .ForMember(dest => dest.RecurrenceDayOfWeek, opt => opt.MapFrom(src => src.RecurrenceDayOfWeek))
            .ForMember(dest => dest.RecurrenceDayOfMonth, opt => opt.MapFrom(src => src.RecurrenceDayOfMonth))
            .ForMember(dest => dest.RecurrenceEndDate, opt => opt.MapFrom(src => src.RecurrenceEndDate))
            .ForMember(dest => dest.RecurrenceCount, opt => opt.MapFrom(src => src.RecurrenceCount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility))
            .ForMember(dest => dest.IsAllDay, opt => opt.MapFrom(src => src.IsAllDay))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

        CreateMap<GoogleCalendarEventDto, EventDto>();

        CreateMap<GoogleCalendarEventDto, Event>();
    }
}
using AutoMapper;
using ItConsultations.Business.Dtos.ConsultationDtos;
using ItConsultations.Business.Dtos.StudentDtos;
using ItConsultations.Business.Dtos.CoachDtos;
using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Entities.Consultation;
using ItConsultations.Business.Entities.Article;


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
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.DisplayName))
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
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

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
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));
    }
}
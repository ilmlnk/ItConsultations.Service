using AutoMapper;
using ItConsultations.Business.Dtos;
using ItConsultations.Business.Entities.Consultation;


namespace ItConsultations.Business.AutoMapperConfiguration;

public class ConsultationsAutoMapperProfile : Profile
{
    public ConsultationsAutoMapperProfile()
    {
        CreateMappings();
    }

    private void CreateMappings()
    {
        CreateMap<Consultation, ConsultationDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl))
            .ForMember(dest => dest.Coach, opt => opt.MapFrom(src => src.Coach));
    }
}
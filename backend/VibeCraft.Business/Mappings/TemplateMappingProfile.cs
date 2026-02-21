using AutoMapper;
using VibeCraft.Models.Entities;
using VibeCraft.Models.ViewModels;

namespace VibeCraft.Models.Mappings
{
    /// <summary>
    /// AutoMapper profile for Template mappings
    /// </summary>
    public class TemplateMappingProfile : Profile
    {
        public TemplateMappingProfile()
        {
            // Entity to ViewModel mappings
            CreateMap<Template, TemplateViewModel>()
                .ForMember(dest => dest.ForEventType, 
                    opt => opt.MapFrom(src => ((EventType)src.ForEventType).ToString()))
                .ForMember(dest => dest.EventPlansCount, 
                    opt => opt.MapFrom(src => src.EventPlans != null ? src.EventPlans.Count : 0));

            CreateMap<Template, TemplateDetailsViewModel>()
                .ForMember(dest => dest.ForEventType, 
                    opt => opt.MapFrom(src => ((EventType)src.ForEventType).ToString()))
                .ForMember(dest => dest.EventPlansCount, 
                    opt => opt.MapFrom(src => src.EventPlans != null ? src.EventPlans.Count : 0));

            // Map for EventPlan within template details

            // ViewModel to Entity mappings
            CreateMap<CreateTemplateViewModel, Template>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ForEventType, 
                    opt => opt.MapFrom(src => (int)src.ForEventType))
                .ForMember(dest => dest.EventPlans, opt => opt.Ignore());

        }
    }
}
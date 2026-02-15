using AutoMapper;
using VibeCraft.Models.Entities;
using VibeCraft.Models.ViewModels;

namespace VibeCraft.Models.Mappings
{
    /// <summary>
    /// AutoMapper profile for EventPlan mappings
    /// </summary>
    public class EventPlanMappingProfile : Profile
    {
        public EventPlanMappingProfile()
        {
            // Entity to ViewModel mappings
            CreateMap<EventPlan, EventPlanViewModel>();

            CreateMap<EventPlan, EventPlanDetailsViewModel>()
                .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.Event != null ? src.Event.Title : string.Empty))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Event != null ? src.Event.EventDate : DateTime.MinValue))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.Event != null ? src.Event.EventType.ToString() : string.Empty))
                .ForMember(dest => dest.TemplateDescription, opt => opt.MapFrom(src => src.Template != null ? src.Template.Description : string.Empty));

            // ViewModel to Entity mappings
            CreateMap<CreateEventPlanViewModel, EventPlan>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.Template, opt => opt.Ignore());

        }
    }
}
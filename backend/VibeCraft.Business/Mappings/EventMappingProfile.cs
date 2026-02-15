using AutoMapper;
using VibeCraft.Models.Entities;
using VibeCraft.Models.ViewModels;

namespace VibeCraft.Models.Mappings
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            // Entity to ViewModel mappings
            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType.ToString()))
                .ForMember(dest => dest.BudgetRange, opt => opt.MapFrom(src => src.BudgetRange.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Event, EventDetailsViewModel>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType.ToString()))
                .ForMember(dest => dest.BudgetRange, opt => opt.MapFrom(src => src.BudgetRange.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Create mappings for related entities if they exist
            CreateMap<EventPlan, EventPlanViewModel>();
            CreateMap<BudgetRange, BudgetViewModel>();


            // ViewModel to Entity mappings
            CreateMap<CreateEventViewModel, Event>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => EventStatus.Planning));
        }
    }
}
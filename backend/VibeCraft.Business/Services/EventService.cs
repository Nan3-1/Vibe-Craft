// Services/EventService.cs
using Microsoft.EntityFrameworkCore;
using System;       
using VibeCraft.Data;
using VibeCraft.Models.Entities; 
using VibeCraft.Models.DTOs;             
using System.Collections.Generic;
using System.Linq;    
using System.Threading.Tasks;
using static VibeCraft.Models.DTOs.Login;

namespace VibeCraft.Business.Services
{
    public interface IEventService
    {
        Task<EventDto> CreateEvent(CreateEventDto dto, int userId);
        Task<EventDto> GetEventById(int id);
        Task<List<EventDto>> GetUserEvents(int userId);
        Task<EventPlanDto> GenerateEventPlan(GenerateTemplateDto dto);
        Task<EventDto> UpdateEventStatus(int eventId, EventStatus status);
        Task<bool> AddParticipant(int eventId, int userId, ParticipantRole role);
        Task<bool> RemoveParticipant(int eventId, int userId);
    }

    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EventDto> CreateEvent(CreateEventDto dto, int userId)
        {
            var @event = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                EventType = dto.EventType,
                EventDate = dto.EventDate,
                ExpectedGuests = dto.ExpectedGuests,
                VibeTheme = dto.VibeTheme,
                LocationDescription = dto.LocationDescription,
                BudgetRange = dto.BudgetRange,
                CreatedById = userId,
                Status = EventStatus.Planning,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            // Create budget
            var budget = new Budget
            {
                EventId = @event.Id,
                TotalAmount = GetDefaultBudget(dto.BudgetRange, dto.ExpectedGuests),
                Currency = "BGN",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Budgets.Add(budget);

            // Add creator as owner participant
            var participant = new EventParticipant
            {
                EventId = @event.Id,
                UserId = userId,
                Role = ParticipantRole.Owner,
                JoinedAt = DateTime.UtcNow
            };

            _context.EventParticipants.Add(participant);
            await _context.SaveChangesAsync();

            return await GetEventById(@event.Id);
        }

        public async Task<EventDto> GetEventById(int id)
        {
            var @event = await _context.Events
                .Include(e => e.CreatedBy)
                .Include(e => e.EventPlan)
                .Include(e => e.Budget)
                .Include(e => e.Participants)
                    .ThenInclude(p => p.User)
                .Include(e => e.Bookings)
                    .ThenInclude(b => b.ServiceId)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
                throw new Exception("Event not found");

            return MapToEventDto(@event);
        }

        public async Task<List<EventDto>> GetUserEvents(int userId)
        {
            var events = await _context.Events
                .Where(e => e.CreatedById == userId || 
                           e.Participants.Any(p => p.UserId == userId))
                .Include(e => e.CreatedBy)
                .Include(e => e.EventPlan)
                .Include(e => e.Budget)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            return events.Select(MapToEventDto).ToList();
        }

        public async Task<EventPlanDto> GenerateEventPlan(GenerateTemplateDto dto)
        {
            // Find matching template
            var template = await _context.Templates
                .Where(t => t.ForEventType == (int)dto.EventType && 
                           t.VibeType.Contains(dto.VibeTheme) &&
                           t.IsActive)
                .OrderByDescending(t => t.IsPremium)
                .FirstOrDefaultAsync();

            if (template == null)
            {
                // Generate default plan based on event type and vibe
                return new EventPlanDto
                {
                    ColorPalette = GetColorPalette(dto.VibeTheme),
                    MusicPreferences = GetMusicSuggestions(dto.EventType, dto.VibeTheme),
                    FoodPreferences = GetFoodSuggestions(dto.EventType, dto.ExpectedGuests),
                    DecorationDetails = GetDecorationTips(dto.EventType, dto.VibeTheme),
                    Template = null
                };
            }

            return new EventPlanDto
            {
                ColorPalette = template.ColorScheme,
                MusicPreferences = GetMusicSuggestions(dto.EventType, dto.VibeTheme),
                FoodPreferences = GetFoodSuggestions(dto.EventType, dto.ExpectedGuests),
                DecorationDetails = $"Based on template: {template.Name}. {GetDecorationTips(dto.EventType, dto.VibeTheme)}",
                AdditionalNotes = template.Description,
                Template = new TemplateDto
                {
                    Id = template.Id,
                    Name = template.Name,
                    Description = template.Description,
                    IsPremium = template.IsPremium,
                    BasePrice = template.BasePrice
                }
            };
        }

        public async Task<EventDto> UpdateEventStatus(int eventId, EventStatus status)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null)
                throw new Exception("Event not found");

            @event.Status = status;
            @event.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GetEventById(eventId);
        }

        public async Task<bool> AddParticipant(int eventId, int userId, ParticipantRole role)
        {
            // Check if already participant
            var existing = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (existing != null)
                return false;

            var participant = new EventParticipant
            {
                EventId = eventId,
                UserId = userId,
                Role = role,
                JoinedAt = DateTime.UtcNow
            };

            _context.EventParticipants.Add(participant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveParticipant(int eventId, int userId)
        {
            var participant = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (participant == null)
                return false;

            _context.EventParticipants.Remove(participant);
            await _context.SaveChangesAsync();
            return true;
        }

        // Helper methods
        private decimal GetDefaultBudget(BudgetRange range, int guests)
        {
            return range switch
            {
                BudgetRange.Standard => guests * 50,
                BudgetRange.Premium => guests * 100,
                BudgetRange.Luxury => guests * 200,
                _ => guests * 50
            };
        }

        private string GetColorPalette(string vibe)
        {
            return vibe.ToLower() switch
            {
                "modern" => "#333333, #FFFFFF, #007ACC, #F5F5F5",
                "minimalist" => "#FFFFFF, #F0F0F0, #CCCCCC, #000000",
                "tropical" => "#FF6B6B, #4ECDC4, #45B7D1, #96CEB4, #FFEAA7",
                "vintage" => "#8B7355, #CD853F, #D2691E, #A0522D, #8B4513",
                _ => "#333333, #FFFFFF, #007ACC"
            };
        }

        private string GetMusicSuggestions(EventType eventType, string vibe)
        {
            var suggestions = new List<string>();
            
            if (eventType == EventType.Wedding)
                suggestions.AddRange(new[] { "Classical", "Jazz", "Romantic Ballads", "Acoustic Covers" });
            else if (eventType == EventType.BirthdayParty)
                suggestions.AddRange(new[] { "Pop Hits", "Dance Music", "80s/90s Classics" });
            
            if (vibe.ToLower().Contains("tropical"))
                suggestions.Add("Reggae, Tropical House, Beach Music");
            
            return string.Join(", ", suggestions.Distinct());
        }

        private string GetFoodSuggestions(EventType eventType, int guests)
        {
            return eventType switch
            {
                EventType.Wedding => $"Formal 3-course meal for {guests} guests",
                EventType.CorporateMeeting => $"Business lunch, coffee break snacks",
                EventType.BirthdayParty => $"Finger foods, birthday cake, snacks",
                EventType.ConcertFestival => $"Food trucks, quick bites, beverages",
                _ => $"Buffet style catering for {guests} guests"
            };
        }

        private string GetDecorationTips(EventType eventType, string vibe)
        {
            return $"For a {vibe} {eventType}, consider decorations that match the theme. ";
        }

        private EventDto MapToEventDto(Event @event)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new EventDto
            {
                Id = @event.Id,
                Title = @event.Title,
                Description = @event.Description,
                EventType = @event.EventType,
                EventDate = @event.EventDate,
                ExpectedGuests = @event.ExpectedGuests,
                VibeTheme = @event.VibeTheme,
                LocationDescription = @event.LocationDescription,
                ActualLocation = @event.ActualLocation,
                BudgetRange = @event.BudgetRange,
                Status = @event.Status,
                CreatedAt = @event.CreatedAt,
                CreatedBy = MapToUserProfileDto(@event.CreatedBy),
                EventPlan = @event.EventPlan != null ? new EventPlanDto
                {
                    ColorPalette = @event.EventPlan.ColorPalette,
                    MusicPreferences = @event.EventPlan.MusicPreferences,
                    FoodPreferences = @event.EventPlan.FoodPreferences,
                    DecorationDetails = @event.EventPlan.DecorationDetails,
                    AdditionalNotes = @event.EventPlan.AdditionalNotes,
                    Template = @event.EventPlan.Template != null ? new TemplateDto
                    {
                        Id = @event.EventPlan.Template.Id,
                        Name = @event.EventPlan.Template.Name,
                        Description = @event.EventPlan.Template.Description
                    } : null
                } : null,
                Budget = @event.Budget != null ? new BudgetDto
                {
                    TotalAmount = @event.Budget.TotalAmount,
                    SpentAmount = @event.Budget.SpentAmount,
                    Currency = @event.Budget.Currency
                } : null,
                Participants = @event.Participants?.Select(p => new ParticipantDto
                {
                    UserId = p.UserId,
                    Role = p.Role,
                    User = MapToUserProfileDto(p.User)
                }).ToList(),
                Bookings = @event.Bookings?.Select(b => new BookingDto
                {
                    Id = b.Id,
                    ServiceName = b.Service?.Name,
                    Quantity = b.Quantity,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    BookingDate = b.BookingDate
                }).ToList()
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        private UserProfileDto MapToUserProfileDto(User user)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return user switch
            {
                RegularUser regular => new UserProfileDto
                {
                    Id = regular.Id,
                    Username = regular.Username,
                    Email = regular.Email,
                    FirstName = regular.FirstName,
                    LastName = regular.LastName,
                    ProfilePicture = regular.ProfilePicture,
                    UserType = "Regular"
                },
                EventPlannerUser planner => new UserProfileDto
                {
                    Id = planner.Id,
                    Username = planner.Username,
                    Email = planner.Email,
                    FirstName = planner.FirstName,
                    LastName = planner.LastName,
                    ProfilePicture = planner.ProfilePicture,
                    UserType = "Planner",
                    Bio = planner.Bio,
                    Specialization = planner.Specialization,
                    Rating = planner.Rating,
                    CompletedEvents = planner.CompletedEvents,
                    CompanyName = planner.CompanyName
                },
                AdminUser admin => new UserProfileDto
                {
                    Id = admin.Id,
                    Username = admin.Username,
                    Email = admin.Email,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    ProfilePicture = admin.ProfilePicture,
                    UserType = "Admin"
                },
                _ => null
            };
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

   
}
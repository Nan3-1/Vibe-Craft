using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeCraft.Models.Entities;

namespace VibeCraft.Business.Interfaces
{
    public interface IEventService
    {
        Task<Event> CreateEventAsync(Event eventToCreate);
        Task<Event> GetEventByIdAsync(int eventId);
        Task<List<Event>> GetEventsByUserIdAsync(int userId);
        //Task<GeneratedPlan> GenerateEventTemplateAsync(GenerateTemplateRequest request);
        Task<Event> UpdateEventDetailsAsync(int eventId, Event updatedEvent);
        Task<Event> UpdateEventStatusAsync(int eventId, EventStatus newStatus);
        Task<bool> AddParticipantToEventAsync(int eventId, int userId);
        Task<bool> RemoveParticipantFromEventAsync(int eventId, int userId);
        Task<Event> UpdateEventAsync(Event eventToUpdate);
        Task<bool> DeleteEventAsync(int eventId);
    }
}
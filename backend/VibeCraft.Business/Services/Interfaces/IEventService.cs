using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VibeCraft.Models.ViewModels;

namespace VibeCraft.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing events
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Gets all events
        /// </summary>
        Task<IEnumerable<EventViewModel>> GetAllEventsAsync();

        /// <summary>
        /// Gets an event by ID
        /// </summary>
        Task<EventDetailsViewModel> GetEventByIdAsync(int id);

        /// <summary>
        /// Creates a new event
        /// </summary>
        Task<EventViewModel> CreateEventAsync(CreateEventViewModel createModel, int createdById);

        /// <summary>
        /// Deletes an event
        /// </summary>
        Task<bool> DeleteEventAsync(int id);

        /// <summary>
        /// Gets events by user ID
        /// </summary>
        Task<IEnumerable<EventViewModel>> GetEventsByUserIdAsync(int userId);

        /// <summary>
        /// Gets events by status
        /// </summary>
        Task<IEnumerable<EventViewModel>> GetEventsByStatusAsync(EventStatus status);


        /// <summary>
        /// Checks if an event exists
        /// </summary>
        Task<bool> EventExistsAsync(int id);
    }
}
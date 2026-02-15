using System.Collections.Generic;
using System.Threading.Tasks;
using VibeCraft.Models.ViewModels;

namespace VibeCraft.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing event plans
    /// </summary>
    public interface IEventPlanService
    {
        /// <summary>
        /// Gets all event plans
        /// </summary>
        Task<IEnumerable<EventPlanViewModel>> GetAllEventPlansAsync();

        /// <summary>
        /// Gets an event plan by ID
        /// </summary>
        Task<EventPlanDetailsViewModel> GetEventPlanByIdAsync(int id);

        /// <summary>
        /// Gets an event plan by event ID
        /// </summary>
        Task<EventPlanViewModel> GetEventPlanByEventIdAsync(int eventId);

        /// <summary>
        /// Creates a new event plan
        /// </summary>
        Task<EventPlanViewModel> CreateEventPlanAsync(CreateEventPlanViewModel createModel);

        /// <summary>
        /// Deletes an event plan
        /// </summary>
        Task<bool> DeleteEventPlanAsync(int id);

        /// <summary>
        /// Checks if an event plan exists
        /// </summary>
        Task<bool> EventPlanExistsAsync(int id);

        /// <summary>
        /// Checks if an event already has a plan
        /// </summary>
        Task<bool> EventHasPlanAsync(int eventId);
    }
}
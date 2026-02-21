using System.Collections.Generic;
using System.Threading.Tasks;
using VibeCraft.Models.Entities;
using VibeCraft.Models.ViewModels;

namespace VibeCraft.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing templates
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Gets all templates with optional filtering
        /// </summary>
        Task<IEnumerable<TemplateViewModel>> GetAllTemplatesAsync(TemplateFilterViewModel filter = null);

        /// <summary>
        /// Gets a template by ID
        /// </summary>
        Task<TemplateDetailsViewModel> GetTemplateByIdAsync(int id);

        /// <summary>
        /// Creates a new template
        /// </summary>
        Task<TemplateViewModel> CreateTemplateAsync(CreateTemplateViewModel createModel);


        /// <summary>
        /// Deletes a template
        /// </summary>
        Task<bool> DeleteTemplateAsync(int id);

        /// <summary>
        /// Gets templates by event type
        /// </summary>
        Task<IEnumerable<TemplateViewModel>> GetTemplatesByEventTypeAsync(EventType eventType);

        /// <summary>
        /// Gets active templates
        /// </summary>
        Task<IEnumerable<TemplateViewModel>> GetActiveTemplatesAsync();

        /// <summary>
        /// Toggles template active status
        /// </summary>
        Task<bool> ToggleTemplateStatusAsync(int id);

        /// <summary>
        /// Checks if a template exists
        /// </summary>
        Task<bool> TemplateExistsAsync(int id);

        /// <summary>
        /// Checks if a template name is unique
        /// </summary>
        Task<bool> IsTemplateNameUniqueAsync(string name, int? excludeId = null);
    }
}
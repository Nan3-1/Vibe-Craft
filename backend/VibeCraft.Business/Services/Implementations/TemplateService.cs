using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VibeCraft.Data;
using VibeCraft.Models.Entities;
using VibeCraft.Models.ViewModels;
using VibeCraft.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace VibeCraft.Services
{
    /// <summary>
    /// Service implementation for managing templates
    /// </summary>
    public class TemplateService : ITemplateService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TemplateService> _logger;

        public TemplateService(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<TemplateService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TemplateViewModel>> GetAllTemplatesAsync(TemplateFilterViewModel filter = null)
        {
            try
            {
                IQueryable<Template> query = _context.Templates
                    .Include(t => t.EventPlans)
                    .AsQueryable();

                // Apply filtering
                if (filter != null)
                {
                    query = ApplyFiltering(query, filter);
                    query = ApplySorting(query, filter);
                }

                var templates = await query.ToListAsync();
                return _mapper.Map<IEnumerable<TemplateViewModel>>(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all templates");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TemplateDetailsViewModel> GetTemplateByIdAsync(int id)
        {
            try
            {
                var template = await _context.Templates
                    .Include(t => t.EventPlans)
                        .ThenInclude(ep => ep.Event)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (template == null)
                {
                    _logger.LogWarning("Template with ID {TemplateId} not found", id);
                    return null;
                }

                return _mapper.Map<TemplateDetailsViewModel>(template);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting template by ID {TemplateId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteTemplateAsync(int id)
        {
            try
            {
                var template = await _context.Templates
                    .Include(t => t.EventPlans)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (template == null)
                {
                    _logger.LogWarning("Template with ID {TemplateId} not found for deletion", id);
                    return false;
                }


                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Template with ID {TemplateId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting template with ID {TemplateId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> TemplateExistsAsync(int id)
        {
            return await _context.Templates.AnyAsync(t => t.Id == id);
        }

        /// <inheritdoc/>
        #region Private Methods

        private IQueryable<Template> ApplyFiltering(IQueryable<Template> query, TemplateFilterViewModel filter)
        {
            // Search by name or description
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(t =>
                    (t.Description != null && t.Description.ToLower().Contains(searchTerm)) ||
                    (t.VibeType != null && t.VibeType.ToLower().Contains(searchTerm)));
            }

            // Filter by event type
            if (filter.EventType.HasValue)
            {
                query = query.Where(t => t.ForEventType == (int)filter.EventType.Value);
            }


            return query;
        }

        private IQueryable<Template> ApplySorting(IQueryable<Template> query, TemplateFilterViewModel filter)
        {


            // Apply sorting based on SortBy and SortOrder
            bool ascending = string.IsNullOrWhiteSpace(filter.SortOrder) ||
                           filter.SortOrder.ToLower() == "asc";

            return filter.SortBy.ToLower() switch
            {
                "eventtype" => ascending ? query.OrderBy(t => t.ForEventType) : query.OrderByDescending(t => t.ForEventType)
            };
        }

        #endregion

        /// <inheritdoc/>
        public async Task<TemplateViewModel> CreateTemplateAsync(CreateTemplateViewModel createModel)
        {
            try
            {
                // Validate the model
                if (createModel == null)
                {
                    throw new ArgumentNullException(nameof(createModel));
                }

                // Check if template name is unique
                if (!await IsTemplateNameUniqueAsync(createModel.VibeType))
                {
                    throw new InvalidOperationException($"A template with the name '{createModel.VibeType}' already exists.");
                }

                // Map the view model to entity
                var template = _mapper.Map<Template>(createModel);

                // Set additional properties
                template.DateTime = DateTime.UtcNow;
                template.EventPlans = new List<EventPlan>();

                // Add to database
                await _context.Templates.AddAsync(template);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Template '{TemplateName}' created successfully with ID {TemplateId}",
                    template.VibeType, template.Id);

                return _mapper.Map<TemplateViewModel>(template);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating template: {TemplateName}", createModel?.VibeType);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TemplateViewModel>> GetTemplatesByEventTypeAsync(EventType eventType)
        {
            try
            {
                var templates = await _context.Templates
                    .Include(t => t.EventPlans)
                    .Where(t => t.ForEventType == (int)eventType)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TemplateViewModel>>(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting templates by event type: {EventType}", eventType);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TemplateViewModel>> GetActiveTemplatesAsync()
        {
            try
            {
                // Assuming "active" templates are those created within the last 30 days
                // You can adjust this logic based on your business requirements
                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

                var templates = await _context.Templates
                    .Include(t => t.EventPlans)
                    .Where(t => t.DateTime >= thirtyDaysAgo)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TemplateViewModel>>(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active templates");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ToggleTemplateStatusAsync(int id)
        {
            try
            {
                var template = await _context.Templates
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (template == null)
                {
                    _logger.LogWarning("Template with ID {TemplateId} not found for status toggle", id);
                    return false;
                }

                // Toggle the status - you might need to add an IsActive property to your Template entity
                // For now, we'll toggle the DateTime to indicate active/inactive status
                // You should add an IsActive boolean property to the Template class for this functionality

                // Option 1: If you have an IsActive property (recommended)
                // template.IsActive = !template.IsActive;

                // Option 2: Alternative using DateTime (temporary solution)
                // This example toggles by setting a future date to indicate "active"
                if (template.DateTime > DateTime.UtcNow.AddDays(-365))
                {
                    template.DateTime = DateTime.UtcNow.AddYears(-1); // Mark as inactive
                }
                else
                {
                    template.DateTime = DateTime.UtcNow; // Mark as active
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Template status toggled for ID {TemplateId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling template status for ID {TemplateId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> IsTemplateNameUniqueAsync(string name, int? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                var query = _context.Templates.Where(t => t.VibeType.ToLower() == name.ToLower());

                if (excludeId.HasValue)
                {
                    query = query.Where(t => t.Id != excludeId.Value);
                }

                return !await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking template name uniqueness: {TemplateName}", name);
                throw;
            }
        }

    }
}
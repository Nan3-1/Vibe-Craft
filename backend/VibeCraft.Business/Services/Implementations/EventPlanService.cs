using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Service implementation for managing event plans
    /// </summary>
    public class EventPlanService : IEventPlanService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EventPlanService> _logger;

        public EventPlanService(
            ApplicationDbContext context, 
            IMapper mapper, 
            ILogger<EventPlanService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventPlanViewModel>> GetAllEventPlansAsync()
        {
            try
            {
                var eventPlans = await _context.EventPlans
                    .Include(ep => ep.Event)
                    .Include(ep => ep.Template)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<EventPlanViewModel>>(eventPlans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all event plans");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<EventPlanDetailsViewModel> GetEventPlanByIdAsync(int id)
        {
            try
            {
                var eventPlan = await _context.EventPlans
                    .Include(ep => ep.Event)
                    .Include(ep => ep.Template)
                    .FirstOrDefaultAsync(ep => ep.Id == id);

                if (eventPlan == null)
                {
                    _logger.LogWarning("Event plan with ID {EventPlanId} not found", id);
                    return null;
                }

                return _mapper.Map<EventPlanDetailsViewModel>(eventPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event plan by ID {EventPlanId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<EventPlanViewModel> GetEventPlanByEventIdAsync(int eventId)
        {
            try
            {
                var eventPlan = await _context.EventPlans
                    .Include(ep => ep.Event)
                    .Include(ep => ep.Template)
                    .FirstOrDefaultAsync(ep => ep.EventId == eventId);

                if (eventPlan == null)
                {
                    _logger.LogWarning("Event plan for event ID {EventId} not found", eventId);
                    return null;
                }

                return _mapper.Map<EventPlanViewModel>(eventPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event plan for event ID {EventId}", eventId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<EventPlanViewModel> CreateEventPlanAsync(CreateEventPlanViewModel createModel)
        {
            try
            {
                // Check if event exists
                var eventExists = await _context.Events.AnyAsync(e => e.Id == createModel.EventId);
                if (!eventExists)
                {
                    _logger.LogWarning("Cannot create plan: Event with ID {EventId} not found", createModel.EventId);
                    throw new ArgumentException($"Event with ID {createModel.EventId} does not exist");
                }

                // Check if event already has a plan
                var hasPlan = await EventHasPlanAsync(createModel.EventId);
                if (hasPlan)
                {
                    _logger.LogWarning("Cannot create plan: Event with ID {EventId} already has a plan", createModel.EventId);
                    throw new InvalidOperationException($"Event with ID {createModel.EventId} already has a plan");
                }

                // Check if template exists if TemplateId is provided
                if (createModel.TemplateId.HasValue)
                {
                    var templateExists = await _context.Templates.AnyAsync(t => t.Id == createModel.TemplateId);
                    if (!templateExists)
                    {
                        _logger.LogWarning("Template with ID {TemplateId} not found", createModel.TemplateId);
                        throw new ArgumentException($"Template with ID {createModel.TemplateId} does not exist");
                    }
                }

                var eventPlan = _mapper.Map<EventPlan>(createModel);

                _context.EventPlans.Add(eventPlan);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Event plan created successfully with ID {EventPlanId} for event ID {EventId}", 
                    eventPlan.Id, createModel.EventId);

                // Load navigation properties for the view model
                await _context.Entry(eventPlan).Reference(ep => ep.Event).LoadAsync();
                if (eventPlan.TemplateId.HasValue)
                {
                    await _context.Entry(eventPlan).Reference(ep => ep.Template).LoadAsync();
                }

                return _mapper.Map<EventPlanViewModel>(eventPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event plan");
                throw;
            }
        }


        /// <inheritdoc/>
        public async Task<bool> DeleteEventPlanAsync(int id)
        {
            try
            {
                var eventPlan = await _context.EventPlans.FindAsync(id);
                if (eventPlan == null)
                {
                    _logger.LogWarning("Event plan with ID {EventPlanId} not found for deletion", id);
                    return false;
                }

                _context.EventPlans.Remove(eventPlan);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Event plan with ID {EventPlanId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event plan with ID {EventPlanId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> EventPlanExistsAsync(int id)
        {
            return await _context.EventPlans.AnyAsync(ep => ep.Id == id);
        }

        /// <inheritdoc/>
        public async Task<bool> EventHasPlanAsync(int eventId)
        {
            return await _context.EventPlans.AnyAsync(ep => ep.EventId == eventId);
        }
    }
}
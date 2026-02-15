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
    /// Service implementation for managing events
    /// </summary>
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(ApplicationDbContext context, IMapper mapper, ILogger<EventService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync()
        {
            try
            {
                var events = await _context.Events
                    .OrderByDescending(e => e.EventDate)
                    .Include(e => e.EventType)
                    .Include(e => e.VibeTheme)
                    .Include(e => e.BudgetRange)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<EventViewModel>>(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all events");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<EventDetailsViewModel> GetEventByIdAsync(int id)
        {
            try
            {
                var @event = await _context.Events
                    .Include(e => e.EventType)
                    .Include(e => e.VibeTheme)
                    .Include(e => e.BudgetRange)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (@event == null)
                {
                    _logger.LogWarning("Event with ID {EventId} not found", id);
                    return null;
                }

                return _mapper.Map<EventDetailsViewModel>(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event by ID {EventId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<EventViewModel> CreateEventAsync(CreateEventViewModel createModel, int createdById)
        {
            try
            {
                var @event = _mapper.Map<Event>(createModel);
                @event.Status = EventStatus.Planning;

                _context.Events.Add(@event);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Event created successfully with ID {EventId} by user {UserId}", @event.Id, createdById);

                // Load the created by user for the view model

                return _mapper.Map<EventViewModel>(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEventAsync(int id)
        {
            try
            {
                var @event = await _context.Events.FindAsync(id);
                if (@event == null)
                {
                    _logger.LogWarning("Event with ID {EventId} not found for deletion", id);
                    return false;
                }

                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Event with ID {EventId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event with ID {EventId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventViewModel>> GetEventsByUserIdAsync(int userId)
        {
            try
            {
                var events = await _context.Events
                    .Include(e => e.EventType)
                    .Include(e => e.VibeTheme)
                    .Include(e => e.BudgetRange)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<EventViewModel>>(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting events for user {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventViewModel>> GetEventsByStatusAsync(EventStatus status)
        {
            try
            {
                var events = await _context.Events
                    .Include(e => e.EventType)
                    .Include(e => e.VibeTheme)
                    .Include(e => e.BudgetRange)
                    .Where(e => e.Status == status)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<EventViewModel>>(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting events with status {Status}", status);
                throw;
            }
        }

        public Task<bool> EventExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>

    }
}
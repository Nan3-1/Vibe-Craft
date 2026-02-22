using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VibeCraft.Models.ViewModels;
using VibeCraft.Services.Interfaces;

namespace VibeCraft.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync();
                return View(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading events index");
                TempData["Error"] = "An error occurred while loading events.";
                return View(Enumerable.Empty<EventViewModel>());
            }
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var @event = await _eventService.GetEventByIdAsync(id);
                if (@event == null)
                {
                    return NotFound();
                }

                return View(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event details for ID {EventId}", id);
                TempData["Error"] = "An error occurred while loading event details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createModel);
            }

            try
            {
                // In a real application, get the current user ID from authentication
                int currentUserId = GetCurrentUserId(); // Implement this method based on your auth system

                var createdEvent = await _eventService.CreateEventAsync(createModel, currentUserId);
                
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction(nameof(Details), new { id = createdEvent.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the event. Please try again.");
                return View(createModel);
            }
        }


        // POST: Events/Edit/5
    
        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var @event = await _eventService.GetEventByIdAsync(id);
                if (@event == null)
                {
                    return NotFound();
                }

                return View(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event for delete, ID {EventId}", id);
                TempData["Error"] = "An error occurred while loading the event for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _eventService.DeleteEventAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                TempData["Success"] = "Event deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event with ID {EventId}", id);
                TempData["Error"] = "An error occurred while deleting the event.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Events/ByStatus/Planning
        public async Task<IActionResult> ByStatus(EventStatus status)
        {
            try
            {
                var events = await _eventService.GetEventsByStatusAsync(status);
                ViewBag.CurrentStatus = status.ToString();
                return View("Index", events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading events by status {Status}", status);
                TempData["Error"] = "An error occurred while loading events by status.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Events/UpdateStatus/5

        #region Helper Methods

        private int GetCurrentUserId()
        {
            // Implement this based on your authentication system
            // For example, if using Identity:
            // return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            // Placeholder - replace with actual implementation
            return 1;
        }

        private bool UserCanEditEvent(string eventCreatorName)
        {
            // Implement permission logic here
            // For example, check if current user is the creator or an admin
            var currentUserName = User.Identity?.Name;
            return currentUserName == eventCreatorName || User.IsInRole("Admin");
        }

        #endregion
    }
}
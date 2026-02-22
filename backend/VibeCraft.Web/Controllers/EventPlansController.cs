using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VibeCraft.Models.ViewModels;
using VibeCraft.Services.Interfaces;

namespace VibeCraft.Controllers
{
    [Authorize]
    public class EventPlansController : Controller
    {
        private readonly IEventPlanService _eventPlanService;
        private readonly IEventService _eventService;
        private readonly ILogger<EventPlansController> _logger;

        public EventPlansController(
            IEventPlanService eventPlanService,
            IEventService eventService,
            ILogger<EventPlansController> logger)
        {
            _eventPlanService = eventPlanService;
            _eventService = eventService;
            _logger = logger;
        }

        // GET: EventPlans
        public async Task<IActionResult> Index()
        {
            try
            {
                var eventPlans = await _eventPlanService.GetAllEventPlansAsync();
                return View(eventPlans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event plans index");
                TempData["Error"] = "An error occurred while loading event plans.";
                return View(Enumerable.Empty<EventPlanViewModel>());
            }
        }

        // GET: EventPlans/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var eventPlan = await _eventPlanService.GetEventPlanByIdAsync(id);
                if (eventPlan == null)
                {
                    return NotFound();
                }

                return View(eventPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event plan details for ID {EventPlanId}", id);
                TempData["Error"] = "An error occurred while loading event plan details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: EventPlans/Create
        public async Task<IActionResult> Create(int? eventId)
        {
            try
            {
                var viewModel = new CreateEventPlanViewModel();
                
                // Populate Events dropdown
                await PopulateEventsDropDownList(eventId);

                // If eventId is provided, check if it already has a plan
                if (eventId.HasValue)
                {
                    var hasPlan = await _eventPlanService.EventHasPlanAsync(eventId.Value);
                    if (hasPlan)
                    {
                        TempData["Warning"] = "This event already has a plan. You can edit the existing plan.";
                        return RedirectToAction("Edit", new { eventId = eventId.Value });
                    }
                    
                    viewModel.EventId = eventId.Value;
                    
                    // Get event details to display
                    var eventDetails = await _eventService.GetEventByIdAsync(eventId.Value);
                    if (eventDetails != null)
                    {
                        ViewBag.EventTitle = eventDetails.Title;
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create event plan form");
                TempData["Error"] = "An error occurred while loading the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: EventPlans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventPlanViewModel createModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateEventsDropDownList(createModel.EventId);
                    return View(createModel);
                }

                // Check if event already has a plan
                if (await _eventPlanService.EventHasPlanAsync(createModel.EventId))
                {
                    ModelState.AddModelError(string.Empty, "This event already has a plan.");
                    await PopulateEventsDropDownList(createModel.EventId);
                    return View(createModel);
                }

                var createdPlan = await _eventPlanService.CreateEventPlanAsync(createModel);
                
                TempData["Success"] = "Event plan created successfully!";
                return RedirectToAction(nameof(Details), new { id = createdPlan.Id });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error creating event plan");
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateEventsDropDownList(createModel.EventId);
                return View(createModel);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business rule violation creating event plan");
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateEventsDropDownList(createModel.EventId);
                return View(createModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event plan");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the event plan. Please try again.");
                await PopulateEventsDropDownList(createModel.EventId);
                return View(createModel);
            }
        }

      

       
        // GET: EventPlans/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eventPlan = await _eventPlanService.GetEventPlanByIdAsync(id);
                if (eventPlan == null)
                {
                    return NotFound();
                }

                return View(eventPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event plan for delete, ID {EventPlanId}", id);
                TempData["Error"] = "An error occurred while loading the event plan for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: EventPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _eventPlanService.DeleteEventPlanAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                TempData["Success"] = "Event plan deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event plan with ID {EventPlanId}", id);
                TempData["Error"] = "An error occurred while deleting the event plan.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: EventPlans/ByEvent/5
        public async Task<IActionResult> ByEvent(int eventId)
        {
            try
            {
                var eventPlan = await _eventPlanService.GetEventPlanByEventIdAsync(eventId);
                if (eventPlan == null)
                {
                    TempData["Info"] = "This event doesn't have a plan yet.";
                    return RedirectToAction("Details", "Events", new { id = eventId });
                }

                return RedirectToAction(nameof(Details), new { id = eventPlan.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding event plan for event ID {EventId}", eventId);
                TempData["Error"] = "An error occurred while finding the event plan.";
                return RedirectToAction("Details", "Events", new { id = eventId });
            }
        }

        #region Helper Methods

        private async Task PopulateEventsDropDownList(int? selectedEventId = null, bool disabled = false)
        {
            var events = await _eventService.GetAllEventsAsync();
            ViewBag.EventList = new SelectList(events, "Id", "Title", selectedEventId);
            ViewBag.EventDropdownDisabled = disabled;
        }

        private async Task PopulateTemplatesDropDownList(int? selectedTemplateId = null)
        {
            // This assumes you have a TemplateService - if not, you'll need to create one
            // For now, we'll use a placeholder or you can implement a simple service
            var templates = await GetTemplatesAsync(); // You'll need to implement this
            ViewBag.TemplateList = new SelectList(templates, "Id", "Name", selectedTemplateId);
        }

        // Temporary method - replace with actual TemplateService
        private async Task<List<SelectListItem>> GetTemplatesAsync()
        {
            // This is a placeholder. Implement proper template service
            return await Task.FromResult(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Select Template (Optional) --" },
                new SelectListItem { Value = "1", Text = "Wedding Template" },
                new SelectListItem { Value = "2", Text = "Corporate Template" },
                new SelectListItem { Value = "3", Text = "Birthday Template" }
            });
        }

        #endregion
    }
}
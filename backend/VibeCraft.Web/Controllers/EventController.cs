using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VibeCraft.Data;
using VibeCraft.Models.Entities;

namespace VibeCraft.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public EventController(
            ILogger<EventController> logger,
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // GET: /Event/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string EventType, int ExpectedGuests, string EventDate, 
                                   string VibeTheme, string LocationDescription, string BudgetRange)
        {
            try
            {
                // Parse the event date
                DateTime parsedEventDate;
                if (!DateTime.TryParse(EventDate, out parsedEventDate))
                {
                    parsedEventDate = DateTime.Now.AddDays(30);
                }

                // Parse enum values
                Enum.TryParse<EventType>(EventType, out var eventTypeEnum);
                Enum.TryParse<BudgetRange>(BudgetRange, out var budgetEnum);

                // Get current user
                var currentUser = await _userManager.GetUserAsync(User);

                // Create new event
                var newEvent = new Event
                {
                    Title = $"{EventType} Event",
                    Description = VibeTheme,
                    EventType = eventTypeEnum,
                    EventDate = parsedEventDate,
                    ExpectedGuests = ExpectedGuests,
                    VibeTheme = VibeTheme,
                    LocationDescription = LocationDescription,
                    BudgetRange = budgetEnum,
                    Status = EventStatus.Planning
                };

                // Save to database
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Event created successfully with ID: {EventId}", newEvent.Id);
                
                TempData["Success"] = "Event created successfully!";
                TempData["EventType"] = EventType;
                TempData["Guests"] = ExpectedGuests.ToString();
                TempData["Theme"] = VibeTheme;
                TempData["Location"] = LocationDescription;
                TempData["Budget"] = BudgetRange;
                
                return RedirectToAction("CreateSuccess");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                ModelState.AddModelError("", "An error occurred while creating the event: " + ex.Message);
                return View();
            }
        }

        // GET: /Event/CreateSuccess
        [HttpGet]
        public IActionResult CreateSuccess()
        {
            return View();
        }

        // GET: /Event/MyEvents
        [Authorize]
        public async Task<IActionResult> MyEvents()
        {
            var events = _context.Events.ToList();
            return View(events);
        }
    }
}
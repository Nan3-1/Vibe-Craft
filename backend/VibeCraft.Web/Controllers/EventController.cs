using Microsoft.AspNetCore.Mvc;
using VibeCraft.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VibeCraft.Web.Controllers
{
    public class EventController : Controller
    {
        // Temporary storage for demo
        private static List<Event> _events = new List<Event>();

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string EventType, int ExpectedGuests, string EventDate, string VibeTheme, string LocationDescription, string BudgetRange)
        {
            // Parse the date
            DateTime parsedDate = DateTime.Now.AddDays(30);
            if (!string.IsNullOrEmpty(EventDate))
            {
                DateTime.TryParse(EventDate, out parsedDate);
            }

            // Create new event
            var newEvent = new Event
            {
                Id = _events.Count + 1,
                Title = EventType + " Celebration",
                Description = VibeTheme,
                ExpectedGuests = ExpectedGuests,
                VibeTheme = VibeTheme,
                LocationDescription = LocationDescription,
                EventDate = parsedDate,
                Status = EventStatus.Planning

            };

            // Save to list
            _events.Add(newEvent);

            TempData["Success"] = "Your event template has been created!";
            return RedirectToAction("MyEvents");
        }

        public IActionResult MyEvents()
        {
            return View(_events);
        }
    }
}
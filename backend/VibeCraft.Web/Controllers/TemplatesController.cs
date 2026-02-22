using Microsoft.AspNetCore.Mvc;

namespace VibeCraft.Web.Controllers
{
    public class TemplatesController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string templateName, string eventType, string description)
        {
            // Just save to TempData if you want to show a message
            TempData["Success"] = $"Template '{templateName}' created successfully!";
            
            // Go straight to profile
            return RedirectToAction("Index", "Profile");
        }
    }
}
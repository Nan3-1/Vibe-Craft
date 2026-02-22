using Microsoft.AspNetCore.Mvc;

namespace VibeCraft.Web.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            // Just set some dummy data
            ViewBag.FirstName = "John";
            ViewBag.LastName = "Doe";
            ViewBag.FullName = "John Doe";
            ViewBag.Email = "john@example.com";
            ViewBag.Phone = "+1234567890";
            ViewBag.EventsCount = 5;
            
            return View();
        }
    }
}
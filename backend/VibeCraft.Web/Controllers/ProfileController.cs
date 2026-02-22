using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibeCraft.Data;
using VibeCraft.Models.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace VibeCraft.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;  // Add this line

        public ProfileController(
            UserManager<User> userManager,
            ApplicationDbContext context)  // Add context to constructor
        {
            _userManager = userManager;
            _context = context;  // Initialize context
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user != null)
            {
                ViewBag.FirstName = user.FirstName ?? "User";
                ViewBag.LastName = user.LastName ?? "";
                ViewBag.FullName = $"{user.FirstName} {user.LastName}".Trim();
                ViewBag.Email = user.Email;
                ViewBag.Phone = user.PhoneNumber;
                
                // Get events count from database
                ViewBag.EventsCount = _context.Events.Count();
                
                // Get user's recent events (optional)
                var userEvents = _context.Events
                    .OrderByDescending(e => e.EventDate)
                    .Take(3)
                    .ToList();
                ViewBag.RecentEvents = userEvents;
            }
            
            return View();
        }
    }
}
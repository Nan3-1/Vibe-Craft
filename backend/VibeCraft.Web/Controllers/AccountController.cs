using Microsoft.AspNetCore.Mvc;

namespace VibeCraft.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password, bool rememberMe = false)
        {
            // Literally just go to profile - no checks, no database, nothing!
            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string fullName, string phoneNumber, string password)
        {
            // Just go to profile
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
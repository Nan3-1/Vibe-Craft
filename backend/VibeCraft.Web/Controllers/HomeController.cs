using Microsoft.AspNetCore.Mvc;

namespace VibeCraft.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult EventDetails()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
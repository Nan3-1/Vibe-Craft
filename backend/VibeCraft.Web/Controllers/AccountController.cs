using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VibeCraft.Models.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

namespace VibeCraft.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Email and password are required.");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in: {Email}", email);
                    return RedirectToAction("Index", "Profile");
                }
                
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                ModelState.AddModelError("", "An error occurred during login.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string fullName, string phoneNumber, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
                {
                    ModelState.AddModelError("", "All fields are required.");
                    return View();
                }

                // Split full name into first and last name
                var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string firstName = nameParts.Length > 0 ? nameParts[0] : fullName;
                string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                var user = new User
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    FirstName = firstName,
                    LastName = lastName
                };

                var result = await _userManager.CreateAsync(user, password);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created: {Email}", email);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Profile");
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error");
                ModelState.AddModelError("", "An error occurred during registration.");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
            return RedirectToAction("Index", "Home");
        }
    }
}
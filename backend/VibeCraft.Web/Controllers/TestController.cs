using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibeCraft.Data;
using VibeCraft.Models.Entities;

namespace VibeCraft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🧪 GET: api/test
        // ПРОСТ ТЕСТ ЗА ПРОВЕРКА
        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok(new { 
                message = "VibeCraft API работи! 🎉", 
                timestamp = DateTime.UtcNow,
                endpoints = new[] {
                    "/api/events",
                    "/api/users",
                    "/api/services",
                    "/api/bookings",
                    "/api/budgets"
                }
            });
        }

        // 🧪 GET: api/test/db
        // ПРОВЕРКА ДАЛИ БАЗАТА РАБОТИ
        [HttpGet("db")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var userCount = await _context.Users.CountAsync();
                var eventCount = await _context.Events.CountAsync();
                var serviceCount = await _context.Services.CountAsync();

                return Ok(new {
                    status = "✅ Базата данни работи!",
                    stats = new {
                        users = userCount,
                        events = eventCount,
                        services = serviceCount
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    status = "❌ Грешка с базата данни!",
                    error = ex.Message
                });
            }
        }

        // 🧪 POST: api/test/seed
        // СЪЗДАЙ ТЕСТОВИ ДАННИ
        [HttpPost("seed")]
        public async Task<IActionResult> SeedTestData()
        {
            // Провери дали вече има данни
            if (await _context.Users.AnyAsync())
            {
                return BadRequest(new { message = "Вече има данни в базата!" });
            }

            // Създай тестов потребител
            var testUser = new RegularUser
            {
                Username = "testuser",
                Email = "test@vibecraft.com",
                PasswordHash = "hashed_password",
                FirstName = "Тест",
                LastName = "Потребител",
                FavoriteEventType = "Wedding"
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

            // Създай тестово събитие
            var testEvent = new Event
            {
                Title = "Тестово Сватбено Тържество",
                Description = "Това е тестово събитие за демонстрация",
                EventType = EventType.Wedding,
                EventDate = DateTime.UtcNow.AddDays(30),
                ExpectedGuests = 100,
                VibeTheme = "Elegant, Romantic",
                LocationDescription = "Розова градина",
                BudgetRange = BudgetRange.Premium,
                CreatedById = testUser.Id
            };

            _context.Events.Add(testEvent);
            await _context.SaveChangesAsync();

            return Ok(new {
                message = "✅ Тестови данни създадени успешно!",
                userId = testUser.Id,
                eventId = testEvent.Id
            });
        }
    }
}
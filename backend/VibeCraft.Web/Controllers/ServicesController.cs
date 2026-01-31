using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibeCraft.Data;
using VibeCraft.Models.Entities;

namespace VibeCraft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📋 GET: api/services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        // 🔍 GET: api/services/category/Catering
        // ТЪРСИ ПО КАТЕГОРИЯ
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesByCategory(ServiceCategory category)
        {
            return await _context.Services
                .Where(s => s.Category == category && s.IsAvailable)
                .ToListAsync();
        }

        // 🔍 GET: api/services/available
        // САМО НАЛИЧНИТЕ УСЛУГИ
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Service>>> GetAvailableServices()
        {
            return await _context.Services
                .Where(s => s.IsAvailable)
                .OrderBy(s => s.Category)
                .ToListAsync();
        }

        // ➕ POST: api/services
        [HttpPost]
        public async Task<ActionResult<Service>> CreateService(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServices), new { id = service.Id }, service);
        }

        // 💸 GET: api/services/price-range?min=100&max=1000
        // УСЛУГИ В ЦЕНОВ ДИАПАЗОН
        [HttpGet("price-range")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesByPriceRange(
            [FromQuery] decimal minPrice = 0, 
            [FromQuery] decimal maxPrice = 10000)
        {
            return await _context.Services
                .Where(s => s.BasePrice >= minPrice && s.BasePrice <= maxPrice && s.IsAvailable)
                .OrderBy(s => s.BasePrice)
                .ToListAsync();
        }
    }
}

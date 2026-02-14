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

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesByCategory(ServiceCategory category)
        {
            return await _context.Services
                .Where(s => s.Category == category && s.IsAvailable)
                .ToListAsync();
        }

        
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Service>>> GetAvailableServices()
        {
            return await _context.Services
                .Where(s => s.IsAvailable)
                .OrderBy(s => s.Category)
                .ToListAsync();
        }

        
        [HttpPost]
        public async Task<ActionResult<Service>> CreateService(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServices), new { id = service.Id }, service);
        }

        
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

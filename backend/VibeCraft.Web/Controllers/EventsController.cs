using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibeCraft.Data;
using VibeCraft.Models.Entities;
using VibeCraft.Web.Helpers;

namespace VibeCraft.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        
        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events
                .Include(e => e.Budget)
                .Include(e => e.EventPlan)
                .ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Budget)
                .Include(e => e.EventPlan)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound(new { message = $"Събитие с ID {id} не е намерено!" });
            }

            return @event;
        }

        
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event @event)
        {
            
            @event.CreatedAt = DateTime.UtcNow;
            @event.UpdatedAt = DateTime.UtcNow;

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, @event);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            @event.UpdatedAt = DateTime.UtcNow;
            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
    
namespace VibeCraft.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("{id}/generate-code")]
        public async Task<IActionResult> GenerateEventCode(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound();

            
            var eventCode = CodeGenerator.GenerateEventCode(@event.EventType.ToString());
            
            return Ok(new
            {
                EventId = id,
                GeneratedCode = eventCode,
                Message = "Кодът е генериран успешно!"
            });
        }

        
        [HttpGet("{id}/parse-vibes")]
        public async Task<IActionResult> ParseEventVibes(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound();

            
            var parsedVibes = TextParser.ParseVibeString(@event.VibeTheme);
            
            return Ok(new
            {
                EventId = id,
                OriginalVibe = @event.VibeTheme,
                ParsedVibes = parsedVibes,
                VibeCount = parsedVibes.Count
            });
        }
    }
}
}
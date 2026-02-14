using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibeCraft.Data;
using VibeCraft.Models.Entities;

namespace VibeCraft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .ToListAsync();
        }

        
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsForEvent(int eventId)
        {
            return await _context.Bookings
                .Where(b => b.EventId == eventId)
                .ToListAsync();
        }

        
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            
            
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, booking);
        }

        
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = BookingStatus.Confirmed;
            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { 
                message = "Резервацията е потвърдена!", 
                bookingId = booking.Id 
            });
        }
    }
}
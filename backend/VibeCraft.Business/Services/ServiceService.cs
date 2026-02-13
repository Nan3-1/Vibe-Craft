using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeCraft.Models.DTOs;
using VibeCraft.Models.Entities;
using VibeCraft.Data;
using Microsoft.EntityFrameworkCore;

namespace VibeCraft.Business.Services
{
    public interface IServiceService
    {
        Task<List<ServiceDto>> GetServices(ServiceCategory? category = null);
        Task<ServiceDto> GetServiceById(int id);
        Task<List<ServiceDto>> SearchServices(string query);
        Task<BookingDto> CreateBooking(BookingRequestDto dto, int userId);
        Task<List<BookingDto>> GetUserBookings(int userId);
        Task<bool> CancelBooking(int bookingId, int userId);
    }

    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;

        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceDto>> GetServices(ServiceCategory? category = null)
        {
            var query = _context.Services.AsQueryable();

            if (category.HasValue)
                query = query.Where(s => s.Category == category.Value);

            var services = await query
                .Where(s => s.IsAvailable)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return services.Select(MapToServiceDto).ToList();
        }

        public async Task<ServiceDto> GetServiceById(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                throw new Exception("Service not found");

            return MapToServiceDto(service);
        }

        public async Task<List<ServiceDto>> SearchServices(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetServices();

            var services = await _context.Services
                .Where(s => s.IsAvailable &&
                           (s.Name.Contains(query) || 
                            s.Description.Contains(query) ||
                            s.ProviderName.Contains(query)))
                .OrderBy(s => s.Name)
                .ToListAsync();

            return services.Select(MapToServiceDto).ToList();
        }

        public async Task<BookingDto> CreateBooking(BookingRequestDto dto, int userId)
        {
            // Verify event exists and user has access
            var eventExists = await _context.Events
                .AnyAsync(e => e.Id == dto.EventId && 
                              (e.CreatedById == userId || 
                               e.Participants.Any(p => p.UserId == userId && 
                                                      p.Role == ParticipantRole.Owner)));

            if (!eventExists)
                throw new Exception("Event not found or unauthorized");

            var service = await _context.Services.FindAsync(dto.ServiceId);
            if (service == null || !service.IsAvailable)
                throw new Exception("Service not available");

            var booking = new Booking
            {
                EventId = dto.EventId,
                ServiceId = dto.ServiceId,
                Quantity = dto.Quantity,
                TotalPrice = service.BasePrice * dto.Quantity,
                Status = BookingStatus.Pending,
                BookingDate = DateTime.UtcNow,
                Notes = dto.Notes
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return new BookingDto
            {
                Id = booking.Id,
                ServiceName = service.Name,
                Quantity = booking.Quantity,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                BookingDate = booking.BookingDate
            };
        }

        public async Task<List<BookingDto>> GetUserBookings(int userId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Service) // use navigation property, not ServiceId
                .Where(b => b.Event.CreatedById == userId || 
                   b.Event.Participants.Any(p => p.UserId == userId && 
                                                p.Role == ParticipantRole.Owner))
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                ServiceName = b.Service.Name, // use navigation
                Quantity = b.Quantity,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                BookingDate = b.BookingDate,
                EventTitle = b.Event?.Title
            }).ToList();
        }

        public async Task<bool> CancelBooking(int bookingId, int userId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.Id == bookingId && 
                                         b.Event.CreatedById == userId);

            if (booking == null)
                return false;

            if (booking.Status == BookingStatus.Confirmed)
            {
                // Check if it's too late to cancel (within 48 hours)
                if ((DateTime.UtcNow - booking.BookingDate).TotalHours < 48)
                    throw new Exception("Cannot cancel confirmed booking within 48 hours");
            }

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        private ServiceDto MapToServiceDto(Service service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Category = service.Category,
                BasePrice = service.BasePrice,
                IsAvailable = service.IsAvailable,
                ProviderName = service.ProviderName,
                ProviderContact = service.ProviderContact
            };
        }
    }
}

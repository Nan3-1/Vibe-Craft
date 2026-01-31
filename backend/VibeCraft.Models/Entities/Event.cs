using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    public class Event
    {
          [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public EventType EventType { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [Range(1, 10000)]
        public int ExpectedGuests { get; set; }

        [Required]
        [MaxLength(100)]
        public string VibeTheme { get; set; } // Modern, Minimalist, Tropical, Vintage

        [MaxLength(500)]
        public string LocationDescription { get; set; }

        [MaxLength(300)]
        public string ActualLocation { get; set; }

        [Required]
        public BudgetRange BudgetRange { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Planning;

        [Required]
        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Навигационни свойства
        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }

        public virtual EventPlan EventPlan { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual ICollection<EventParticipant> Participants { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }

    public enum EventType
    {
        Wedding = 0,
        CorporateMeeting = 1,
        BirthdayParty = 2,
        ConcertFestival = 3,
        Other = 4
    }

    public enum BudgetRange
    {
        Standard = 0,      // Budget Friendly
        Premium = 1,       // Mid Range
        Luxury = 2         // High End
    }

    public enum EventStatus
    {
        Planning = 0,
        Confirmed = 1,
        Completed = 2,
        Cancelled = 3
    }
    
}
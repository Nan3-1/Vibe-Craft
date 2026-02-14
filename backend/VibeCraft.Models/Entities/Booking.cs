using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    public class Booking
    {
        
        [Key]
        public int Id { get; set; }

        public Service Service { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        [Range(0, 1000000)]
        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [MaxLength(500)]
        public string Notes { get; set; }

        
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service ServiceDetails { get; set; }
    }

    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2,
        Completed = 3
    
    }
}
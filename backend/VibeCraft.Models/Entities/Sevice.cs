using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    public class Sevice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public ServiceCategory Category { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal BasePrice { get; set; }

        public bool IsAvailable { get; set; } = true;

        [MaxLength(150)]
        public string ProviderName { get; set; }

        [MaxLength(100)]
        public string ProviderContact { get; set; }

        // Навигационни свойства
        public virtual ICollection<Booking> Bookings { get; set; }
    }

    public enum ServiceCategory
    {
        Catering = 0,
        Venue = 1,
        Entertainment = 2,
        Decoration = 3,
        Photography = 4,
        Transportation = 5,
        Other = 6
    }
    
}
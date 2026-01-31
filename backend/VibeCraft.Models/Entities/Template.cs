using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VibeCraft.Models.Entities
{
    public class Template
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int ForEventType { get; set; } // Съответства на EventType enum

        [MaxLength(100)]
        public string ColorScheme { get; set; }

        [MaxLength(50)]
        public string VibeType { get; set; }

        public bool IsPremium { get; set; } = false;

        [Range(0, 10000)]
        public decimal BasePrice { get; set; } = 0.00m;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Навигационни свойства
        public virtual ICollection<EventPlan> EventPlans { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    public class Budget
    {
         [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        [Range(0, 10000000)]
        public decimal TotalAmount { get; set; }

        [Range(0, 10000000)]
        public decimal SpentAmount { get; set; } = 0.00m;

        [MaxLength(3)]
        public string Currency { get; set; } = "BGN";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Навигационни свойства
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }
}
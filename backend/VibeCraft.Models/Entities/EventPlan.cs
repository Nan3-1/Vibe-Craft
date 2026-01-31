using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    public class EventPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        public int? TemplateId { get; set; }

        [MaxLength(200)]
        public string ColorPalette { get; set; }

        [MaxLength(300)]
        public string MusicPreferences { get; set; }

        [MaxLength(500)]
        public string FoodPreferences { get; set; }

        [MaxLength(1000)]
        public string DecorationDetails { get; set; }

        public string AdditionalNotes { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // Навигационни свойства
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }
    }
}
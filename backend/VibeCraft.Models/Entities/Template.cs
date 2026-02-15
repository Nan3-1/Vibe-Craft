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
        public int ForEventType { get; set; } 

        [Required]
        public DateTime? DateTime { get; set; } 
        
        [Required]
        public required string Guest {get; set;}

        [MaxLength(50)]
        public required string VibeType { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }
        public required string BudgetRange { get; set; }
        
        public required virtual ICollection<EventPlan> EventPlans { get; set; }
    }
}
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

        
        [ForeignKey("EventId")]
        public required virtual Event Event { get; set; }

        [ForeignKey("TemplateId")]
        public required virtual Template Template { get; set; }
    }
}
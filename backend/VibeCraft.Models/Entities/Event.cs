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
        public  string Description { get; set; }

        [Required]
        public EventType EventType {get; set;}

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [Range(1, 10000)]
        public int ExpectedGuests { get; set; }

        [Required]
        [MaxLength(100)]
        public  string VibeTheme { get; set; } 

        [MaxLength(500)]
        public  string LocationDescription { get; set; }


        [Required]
        public BudgetRange BudgetRange { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Planning;

    }
    
}
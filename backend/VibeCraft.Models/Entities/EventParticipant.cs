using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    public class EventParticipant
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public ParticipantRole Role { get; set; } = ParticipantRole.Guest;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    public enum ParticipantRole
    {
        Owner = 0,
        CoPlanner = 1,
        Guest = 2,
        Vendor = 3
    }
    
}
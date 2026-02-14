using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    
    public abstract class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength]
        public string ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        
        public virtual ICollection<Event> CreatedEvents { get; set; }
        public virtual ICollection<EventParticipant> ParticipatedEvents { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }

    
    public class RegularUser : User
    {
        
        [MaxLength(100)]
        public string FavoriteEventType { get; set; }
        
        public DateTime? LastEventCreated { get; set; }
        public int EventsCreatedCount { get; set; } = 0;

        public bool CanCreateNewEvent()
        {
            return EventsCreatedCount < 10; 
        }
    }

    
    public class EventPlannerUser : User
    {
        
        [MaxLength(500)]
        public string Bio { get; set; }
        
        public int ExperienceYears { get; set; }
        
        [MaxLength(100)]
        public string Specialization { get; set; }
        
        [Range(0, 5)]
        public decimal Rating { get; set; } = 0.0m;
        
        public int CompletedEvents { get; set; } = 0;
        
        public bool IsCertified { get; set; } = false;
        
        [MaxLength(200)]
        public string CompanyName { get; set; }
        
        
        public decimal CalculateSuccessRate()
        {
            return CompletedEvents > 0 ? (decimal)CompletedEvents / (CompletedEvents + 5) * 100 : 0;
        }
    }

    
    public class AdminUser : User
    {
        public DateTime LastLogin { get; set; }
        public DateTime? LastSystemBackup { get; set; }
        
        public bool CanManageUsers { get; set; } = true;
        public bool CanManageEvents { get; set; } = true;
        public bool CanManageTemplates { get; set; } = true;
        public bool CanManageServices { get; set; } = true;
        
        [MaxLength(50)]
        public string AdminLevel { get; set; } = "Basic"; 
        
        public int ManagedUsersCount { get; set; } = 0;
        
        
        public bool HasFullAccess()
        {
            return AdminLevel == "Super" || AdminLevel == "System";
        }
    }
}
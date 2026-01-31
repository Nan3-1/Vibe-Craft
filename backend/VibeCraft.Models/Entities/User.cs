using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeCraft.Models.Entities
{
    // ОСНОВЕН (БАЗОВ) клас
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

        // Общи полета за ВСИЧКИ
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

        // Навигационни свойства
        public virtual ICollection<Event> CreatedEvents { get; set; }
        public virtual ICollection<EventParticipant> ParticipatedEvents { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }

    // КЛАС за ОБИКНОВЕН потребител
    public class RegularUser : User
    {
        // Специфични полета само за RegularUser
        [MaxLength(100)]
        public string FavoriteEventType { get; set; }
        
        public DateTime? LastEventCreated { get; set; }
        public int EventsCreatedCount { get; set; } = 0;
        
        // Допълнителни методи
        public bool CanCreateNewEvent()
        {
            return EventsCreatedCount < 10; // Примерно ограничение
        }
    }

    // КЛАС за ОРГАНИЗАТОР
    public class EventPlannerUser : User
    {
        // Специфични полета само за EventPlannerUser
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
        
        // Допълнителни методи
        public decimal CalculateSuccessRate()
        {
            return CompletedEvents > 0 ? (decimal)CompletedEvents / (CompletedEvents + 5) * 100 : 0;
        }
    }

    // КЛАС за АДМИНИСТРАТОР
    public class AdminUser : User
    {
        // Специфични полета само за AdminUser
        public DateTime LastLogin { get; set; }
        public DateTime? LastSystemBackup { get; set; }
        
        public bool CanManageUsers { get; set; } = true;
        public bool CanManageEvents { get; set; } = true;
        public bool CanManageTemplates { get; set; } = true;
        public bool CanManageServices { get; set; } = true;
        
        [MaxLength(50)]
        public string AdminLevel { get; set; } = "Basic"; // Basic, Super, System
        
        public int ManagedUsersCount { get; set; } = 0;
        
        // Допълнителни методи
        public bool HasFullAccess()
        {
            return AdminLevel == "Super" || AdminLevel == "System";
        }
    }
}
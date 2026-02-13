using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VibeCraft.Models.Entities;
using static VibeCraft.Models.DTOs.Login;

namespace VibeCraft.Models.DTOs
{
    public class CreateEventDto
    {

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public EventType EventType { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [Range(1, 10000)]
        public int ExpectedGuests { get; set; }

        [Required]
        [MaxLength(100)]
        public string VibeTheme { get; set; }

        [MaxLength(500)]
        public string LocationDescription { get; set; }

        [Required]
        public BudgetRange BudgetRange { get; set; }
    }

        public class EventDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public EventType EventType { get; set; }
            public DateTime EventDate { get; set; }
            public int ExpectedGuests { get; set; }
            public string VibeTheme { get; set; }
            public string LocationDescription { get; set; }
            public string ActualLocation { get; set; }
            public BudgetRange BudgetRange { get; set; }
            public EventStatus Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public UserProfileDto CreatedBy { get; set; }
            public EventPlanDto EventPlan { get; set; }
            public BudgetDto Budget { get; set; }
            public List<ParticipantDto> Participants { get; set; }
            public List<BookingDto> Bookings { get; set; }
        }
    

    public class BudgetDto
    {
        public decimal SpentAmount;
        public string Currency;

        public decimal TotalAmount { get; set; }
    }

    public class EventPlanDto
    {
        public string ColorPalette { get; set; }
        public string MusicPreferences { get; set; }
        public string FoodPreferences { get; set; }
        public string DecorationDetails { get; set; }
        public string AdditionalNotes { get; set; }
        public GenerateTemplateDto Template { get; set; }
    }

    public class GenerateTemplateDto
    {
        [Required]
        public EventType EventType { get; set; }

        [Required]
        [Range(1, 10000)]
        public int ExpectedGuests { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string VibeTheme { get; set; }

        [MaxLength(500)]
        public string LocationDescription { get; set; }

        [Required]
        public BudgetRange BudgetRange { get; set; }

        public decimal? BudgetAmount { get; set; }
        public int Id { get; set; }
    }
    
}
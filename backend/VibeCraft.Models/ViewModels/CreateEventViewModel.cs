using System;
using System.ComponentModel.DataAnnotations;

namespace VibeCraft.Models.ViewModels
{
    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public required string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Event type is required")]
        public EventType EventType { get; set; }

        [Required(ErrorMessage = "Event date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Expected guests is required")]
        [Range(1, 10000, ErrorMessage = "Expected guests must be between 1 and 10000")]
        [Display(Name = "Expected Guests")]
        public int ExpectedGuests { get; set; }

        [Required(ErrorMessage = "Vibe theme is required")]
        [MaxLength(100, ErrorMessage = "Vibe theme cannot exceed 100 characters")]
        [Display(Name = "Vibe Theme")]
        public required string VibeTheme { get; set; }

        [MaxLength(500, ErrorMessage = "Location description cannot exceed 500 characters")]
        [Display(Name = "Location Description")]
        public required string LocationDescription { get; set; }

        [MaxLength(300, ErrorMessage = "Actual location cannot exceed 300 characters")]
        [Display(Name = "Actual Location")]
        public required string ActualLocation { get; set; }

        [Required(ErrorMessage = "Budget range is required")]
        [Display(Name = "Budget Range")]
        public BudgetRange BudgetRange { get; set; }
    }
}
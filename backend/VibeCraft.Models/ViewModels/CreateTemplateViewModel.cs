using System;
using System.ComponentModel.DataAnnotations;

namespace VibeCraft.Models.ViewModels
{
    /// <summary>
    /// View model for creating a new template
    /// </summary>
    public class CreateTemplateViewModel
    {

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Event type is required")]
        [Display(Name = "Event Type")]
        public EventType ForEventType { get; set; }

        [MaxLength(50, ErrorMessage = "Vibe type cannot exceed 50 characters")]
        [Display(Name = "Vibe Type")]
        public string VibeType { get; set; }
        [MaxLength(100, ErrorMessage = "Budget range cannot exceed 100 characters")]
        [Display(Name = "Budget Range")]
        public string BudgetRange { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace VibeCraft.Models.ViewModels
{

    public class CreateEventPlanViewModel
    {
        [Required(ErrorMessage = "Event ID is required")]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [Display(Name = "Template")]
        public int? TemplateId { get; set; }
        public string AdditionalNotes { get; set; } = string.Empty;
    }
}
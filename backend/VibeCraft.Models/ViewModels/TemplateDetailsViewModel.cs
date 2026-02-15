using System;
using System.Collections.Generic;

namespace VibeCraft.Models.ViewModels
{
    /// <summary>
    /// View model for displaying detailed template information
    /// </summary>
    public class TemplateDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ForEventType { get; set; }
        public string VibeType { get; set; }
        public string BudgetRange { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        
        // Related data
        public List<EventPlanViewModel> EventPlans { get; set; }
        public int EventPlansCount { get; set; }
    }
}
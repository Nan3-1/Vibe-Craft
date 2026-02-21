using System;

namespace VibeCraft.Models.ViewModels
{
    /// <summary>
    /// View model for displaying template information
    /// </summary>
    public class TemplateViewModel
    {
        public int Id { get; set; }
        public required string ForEventType { get; set; }
        public required string Description { get; set;}
        public required string VibeType { get; set; }
        public required string BudgetRange { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int EventPlansCount { get; set; }
    }
}
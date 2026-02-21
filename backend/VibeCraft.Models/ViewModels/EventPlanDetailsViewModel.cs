using System;

namespace VibeCraft.Models.ViewModels
{
    /// <summary>
    /// View model for displaying detailed event plan information
    /// </summary>
    public class EventPlanDetailsViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public required string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public required string EventType { get; set; }
        
        public int? TemplateId { get; set; }
        public required string TemplateName { get; set; }
        public required string TemplateDescription { get; set; }
        
        public DateTime GeneratedAt { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace VibeCraft.Models.ViewModels
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string EventType { get; set; }
        public required DateTime EventDate { get; set; }
        public required int ExpectedGuests { get; set; }
        public required string VibeTheme { get; set; }
        public required string LocationDescription { get; set; }
        public required string BudgetRange { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Related data
        public required EventPlanViewModel EventPlan { get; set; }
        public required BudgetViewModel Budget { get; set; }
    }

    // Supporting view models for related entities
    public class EventPlanViewModel
    {
        public int Id { get; set; }
        public required string PlanDetails { get; set; }
        // Add other properties as needed
    }

    public class BudgetViewModel
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        // Add other properties as needed
    }

}
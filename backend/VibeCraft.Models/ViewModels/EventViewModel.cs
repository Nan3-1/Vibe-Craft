using System;
using System.Collections.Generic;

namespace VibeCraft.Models.ViewModels
{
    public class EventViewModel
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
        public DateTime UpdatedAt { get; set; }
    }
}
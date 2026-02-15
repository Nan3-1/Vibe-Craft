using System;
using System.Collections.Generic;


namespace VibeCraft.Models.ViewModels
{
    public class EventPlantViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public required string EventTitle { get; set; }
        public int? TemplateId { get; set; }
        public required string TemplateName { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
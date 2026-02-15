using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VibeCraft.Models.ViewModels
{
    /// <summary>
    /// View model for filtering templates
    /// </summary>
    public class TemplateFilterViewModel
    {
        public string SearchTerm { get; set; }
        public EventType? EventType { get; set; }
        public bool? IsPremium { get; set; }
        public bool? IsActive { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }

        public List<SelectListItem> EventTypeOptions { get; set; }
        public List<SelectListItem> SortOptions { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VibeCraft.Models.Entities;

namespace VibeCraft.Models.DTOs
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ServiceCategory Category { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
        public string ProviderName { get; set; }
        public string ProviderContact { get; set; }
    }

    public class BookingRequestDto
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        [MaxLength(500)]
        public string Notes { get; set; }
    }
    
        public class TemplateDto : GenerateTemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class BookingDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
         public string EventTitle { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime BookingDate { get; set; }
        
    }

    public class ParticipantDto
    {
        public int UserId { get; set; }
        public ParticipantRole Role { get; set; }
        public object User { get; set; }
    }
}
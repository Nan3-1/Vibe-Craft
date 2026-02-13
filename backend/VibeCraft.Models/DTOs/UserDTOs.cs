using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace VibeCraft.Models.DTOs
{
    public class Login
    {
        public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string UserType { get; set; } // "Regular", "Planner", "Admin"

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        
        // For EventPlanner
        public string Bio { get; set; }
        public string Specialization { get; set; }
        public string CompanyName { get; set; }
    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string UserType { get; set; }
        public string Bio { get; set; }
        public string Specialization { get; set; }
        public decimal Rating { get; set; }
        public int CompletedEvents { get; set; }
        public string CompanyName { get; set; }
    }
    }
}
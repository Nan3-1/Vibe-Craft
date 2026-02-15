using Microsoft.EntityFrameworkCore;
using VibeCraft.Models.Entities;

namespace VibeCraft.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            
            context.Database.EnsureCreated();

            
            if (context.Users.Any())
            {
                return; 
            }

            Console.WriteLine("Starting database seeding...");

            
            var users = new List<User>
            {
                new User
                {
                    FirstName = "Maria",
                    LastName = "Ivanova",
                    Email = "maria.ivanova@example.com",
                    PhoneNumber = "0888123456",

                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
            Console.WriteLine("✓ Users added");

            
            var templates = new List<Template>
            {
                
            };

            context.Templates.AddRange(templates);
            context.SaveChanges();
            Console.WriteLine("✓ Templates added");

            
            var events = new List<Event>
            {
                new Event
                {
                    Title = "Elegant Garden Wedding",
                    Description = "Romantic garden wedding with floral decorations.",
                    EventType = EventType.Wedding,
                    EventDate = new DateTime(2026, 6, 15, 16, 0, 0),
                    ExpectedGuests = 150,
                    VibeTheme = "Elegant Garden Romance",
                    LocationDescription = "Rosewood Gardens with outdoor ceremony space",
                    BudgetRange = BudgetRange.Premium,
                    Status = EventStatus.Confirmed
                },
                new Event
                {
                    Title = "Corporate Annual Meeting",
                    Description = "Annual corporate meeting with presentations.",
                    EventType = EventType.CorporateMeeting,
                    EventDate = new DateTime(2026, 3, 20, 9, 0, 0),
                    ExpectedGuests = 80,
                    VibeTheme = "Modern Professional",
                    LocationDescription = "Conference center with AV equipment",
                    BudgetRange = BudgetRange.Standard,
                    Status = EventStatus.Planning
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
            Console.WriteLine("✓ Events added");

            Console.WriteLine("✅ Database seeding completed successfully!");
        }
    }
}
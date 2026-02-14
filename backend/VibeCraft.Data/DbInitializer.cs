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
                
                new AdminUser
                {
                    Username = "admin",
                    Email = "admin@vibecraft.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    FirstName = "Системен",
                    LastName = "Администратор",
                    PhoneNumber = "+359 88 123 4567",
                    ProfilePicture = "admin-profile.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CanManageUsers = true,
                    CanManageEvents = true,
                    CanManageTemplates = true,
                    CanManageServices = true,
                    AdminLevel = "Super",
                    LastLogin = DateTime.UtcNow
                },
                
                
                new EventPlannerUser
                {
                    Username = "sarah.mitchell",
                    Email = "sarah@vibecraft.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Sarah123!"),
                    FirstName = "Sarah",
                    LastName = "Mitchell",
                    PhoneNumber = "+359 88 765 4321",
                    ProfilePicture = "sarah-profile.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Bio = "Passionate event planner with 5+ years of experience creating memorable celebrations.",
                    ExperienceYears = 5,
                    Specialization = "Weddings & Corporate Events",
                    Rating = 4.8m,
                    CompletedEvents = 42,
                    IsCertified = true,
                    CompanyName = "VibeCraft Events"
                },
                
                
                new RegularUser
                {
                    Username = "ivan.petrov",
                    Email = "ivan@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ivan123!"),
                    FirstName = "Иван",
                    LastName = "Петров",
                    PhoneNumber = "+359 87 654 3210",
                    ProfilePicture = "ivan-profile.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    FavoriteEventType = "Birthday Party",
                    EventsCreatedCount = 3,
                    LastEventCreated = DateTime.UtcNow.AddDays(-30)
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
            Console.WriteLine("✓ Users added");

            
            var services = new List<Service>
            {
                new Service
                {
                    Name = "Premium Catering Service",
                    Description = "Full-service catering with customized menu options.",
                    Category = ServiceCategory.Catering,
                    BasePrice = 2500.00m,
                    IsAvailable = true,
                    ProviderName = "Elite Catering Co.",
                    ProviderContact = "orders@elitecatering.com"
                },
                new Service
                {
                    Name = "Rosewood Garden Venue",
                    Description = "Elegant outdoor garden venue perfect for weddings.",
                    Category = ServiceCategory.Venue,
                    BasePrice = 3500.00m,
                    IsAvailable = true,
                    ProviderName = "Rosewood Gardens",
                    ProviderContact = "bookings@rosewoodgardens.com"
                },
                new Service
                {
                    Name = "Live Band Entertainment",
                    Description = "Professional 5-piece live band with 200+ songs.",
                    Category = ServiceCategory.Entertainment,
                    BasePrice = 1800.00m,
                    IsAvailable = true,
                    ProviderName = "SoundWave Band",
                    ProviderContact = "info@soundwaveband.com"
                }
            };

            context.Services.AddRange(services);
            context.SaveChanges();
            Console.WriteLine("✓ Services added");

            
            var templates = new List<Template>
            {
                new Template
                {
                    Name = "Elegant Garden Wedding",
                    Description = "Complete template for elegant garden weddings.",
                    ForEventType = (int)EventType.Wedding,
                    ColorScheme = "Rose Gold, Sage Green, Ivory",
                    VibeType = "Romantic, Elegant, Natural",
                    IsPremium = true,
                    BasePrice = 299.99m,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new Template
                {
                    Name = "Modern Corporate Meeting",
                    Description = "Professional template for corporate meetings.",
                    ForEventType = (int)EventType.CorporateMeeting,
                    ColorScheme = "Navy Blue, Silver, White",
                    VibeType = "Professional, Modern, Clean",
                    IsPremium = false,
                    BasePrice = 149.99m,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
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
                    ActualLocation = "Rosewood Gardens, Sofia",
                    BudgetRange = BudgetRange.Premium,
                    Status = EventStatus.Confirmed,
                    CreatedById = users[1].Id, 
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
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
                    ActualLocation = "Business Center Sofia",
                    BudgetRange = BudgetRange.Standard,
                    Status = EventStatus.Planning,
                    CreatedById = users[2].Id, // Ivan Petrov
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
            Console.WriteLine("✓ Events added");

            Console.WriteLine("✅ Database seeding completed successfully!");
        }
    }
}
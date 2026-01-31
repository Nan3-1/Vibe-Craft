using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VibeCraft.Data.Persistance;

// VibeCraft.Data/ApplicationDbContext.cs

using Microsoft.EntityFrameworkCore;
using VibeCraft.Models.Entities;

namespace VibeCraft.Data
{
    public class ApplicationDbContext : DbContext
    {
        // КОНСТРУКТОР (задължителен)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DBSET-ове (таблици в базата данни)
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventPlan> EventPlans { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        // МЕТОД за конфигуриране на модела
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Конфигурация за User наследяване
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<RegularUser>("Regular")
                .HasValue<EventPlannerUser>("Planner")
                .HasValue<AdminUser>("Admin");

            // 2. Конфигурация за Many-to-Many връзки
            // EventParticipants (Event <-> User)
            modelBuilder.Entity<EventParticipant>()
                .HasKey(ep => new { ep.EventId, ep.UserId });

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.User)
                .WithMany(u => u.ParticipatedEvents)
                .HasForeignKey(ep => ep.UserId);

            // 3. Уникални полета
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // 4. Връзка 1:1 (Event -> EventPlan)
            modelBuilder.Entity<EventPlan>()
                .HasOne(ep => ep.Event)
                .WithOne(e => e.EventPlan)
                .HasForeignKey<EventPlan>(ep => ep.EventId);

            // 5. Връзка 1:1 (Event -> Budget)
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Event)
                .WithOne(e => e.Budget)
                .HasForeignKey<Budget>(b => b.EventId);

            // 6. Индекси за бързо търсене
            modelBuilder.Entity<Event>()
                .HasIndex(e => e.EventType);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.Status);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.EventDate);

            modelBuilder.Entity<Service>()
                .HasIndex(s => s.Category);

            // 7. Дефиниране на decimal прецизност
            modelBuilder.Entity<Budget>()
                .Property(b => b.TotalAmount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Budget>()
                .Property(b => b.SpentAmount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Service>()
                .Property(s => s.BasePrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(10, 2);

            // 8. Стойности по подразбиране
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Event>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Event>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Booking>()
                .Property(b => b.BookingDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
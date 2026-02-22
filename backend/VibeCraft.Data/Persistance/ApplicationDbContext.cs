using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VibeCraft.Models.Entities;

namespace VibeCraft.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>  // Change to IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<EventPlan> EventPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // This is important for Identity tables

            // Configure enum conversions
            modelBuilder.Entity<Event>()
                .Property(e => e.EventType)
                .HasConversion<int>();

            modelBuilder.Entity<Event>()
                .Property(e => e.BudgetRange)
                .HasConversion<int>();

            modelBuilder.Entity<Event>()
                .Property(e => e.Status)
                .HasConversion<int>();

            // Configure indexes
            modelBuilder.Entity<Event>()
                .HasIndex(e => e.EventType);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.Status);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.EventDate);

            // Configure relationships
            modelBuilder.Entity<EventPlan>()
                .HasOne(ep => ep.Event)
                .WithMany()
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventPlan>()
                .HasOne(ep => ep.Template)
                .WithMany(t => t.EventPlans)
                .HasForeignKey(ep => ep.TemplateId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
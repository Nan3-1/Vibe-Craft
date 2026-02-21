using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using VibeCraft.Models.Entities;


namespace VibeCraft.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<EventPlan> EventPlans { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType");




            
            modelBuilder.Entity<Event>()
                .HasIndex(e => e.EventType);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.Status);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.EventDate);
        }
    }
}
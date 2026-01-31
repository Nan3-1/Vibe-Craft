// VibeCraft.Data/Configurations/EventConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeCraft.Models.Entities;

namespace VibeCraft.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
            
            // Връзки
            builder.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Индекси
            builder.HasIndex(e => e.EventType);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.EventDate);
            builder.HasIndex(e => e.CreatedById);
            
            // Стойности по подразбиране
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(e => e.Status)
                .HasDefaultValue(EventStatus.Planning);
        }
    }
}
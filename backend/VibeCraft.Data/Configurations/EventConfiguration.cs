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
        
            
            // Индекси
            builder.HasIndex(e => e.EventType);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.EventDate);
            
            builder.Property(e => e.Status)
                .HasDefaultValue(EventStatus.Planning);
        }
    }
}
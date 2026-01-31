// VibeCraft.Data/Configurations/UserConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeCraft.Models.Entities;

namespace VibeCraft.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Основна таблица
            builder.ToTable("Users");
            
            // Дискриминатор за наследяване
            builder.HasDiscriminator<string>("UserType")
                .HasValue<RegularUser>("Regular")
                .HasValue<EventPlannerUser>("Planner")
                .HasValue<AdminUser>("Admin");
            
            // Уникални полета
            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
            
            // Стойности по подразбиране
            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);
        }
    }
}
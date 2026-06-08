using FiapCloudGames.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Infrastructure.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
             builder
                .ToTable("Users")
                .HasKey(u => u.Id);

            builder
                .Property(u => u.Id)
                .IsRequired();

            builder
                .Property(u => u.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder
                .Property(u => u.Password)
                .HasMaxLength(256)
                .IsRequired();

            builder
                .Property(u => u.Role)
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(u => u.Status)
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(u => u.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder
                .HasMany(u => u.UsersGames)
                .WithOne(ug => ug.user)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

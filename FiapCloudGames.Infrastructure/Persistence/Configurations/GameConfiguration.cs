using FiapCloudGames.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Infrastructure.Persistence.Configurations
{
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Game");

            builder.HasKey(j => j.Id);

            builder.Property(j => j.Name)
                .IsRequired()
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(j => j.Category)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(j => j.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(j => j.Description)
                .IsRequired()
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);
        }
    }
}

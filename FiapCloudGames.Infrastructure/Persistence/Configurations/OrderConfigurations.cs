using FiapCloudGames.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Infrastructure.Persistence.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.Property(o => o.GameId)
                .IsRequired();

            builder.Property(o => o.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.PurchaseDate)
                .IsRequired();

            builder.Property(o => o.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.HasIndex(o => o.UserId);

            builder.HasIndex(o => o.GameId);

            builder.HasIndex(o => o.Status);
        }
    }
}


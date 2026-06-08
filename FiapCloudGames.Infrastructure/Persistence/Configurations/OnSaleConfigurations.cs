using FiapCloudGames.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Infrastructure.Persistence.Configurations
{
    public class OnSaleConfiguration : IEntityTypeConfiguration<OnSale>
    {
        public void Configure (EntityTypeBuilder<OnSale> builder)
        {
            builder.ToTable("OnSales");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.DiscountPercentage)
                   .HasColumnType("decimal(5,2)")
                   .IsRequired();

            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.EndDate).IsRequired();

            builder.HasOne(p => p.Game)
             .WithMany(g => g.OnSales)
             .HasForeignKey(p => p.GameId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

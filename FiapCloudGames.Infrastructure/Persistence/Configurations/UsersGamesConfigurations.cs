using FiapCloudGames.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Infrastructure.Persistence.Configurations
{
    public class UsersGamesConfigurations : IEntityTypeConfiguration<UsersGames>
    {
        public void Configure(EntityTypeBuilder<UsersGames> builder)
    {
            builder
                .ToTable("UsersGames");

            //  Chave composta
            builder
                .HasKey(ug => new { ug.UserId, ug.GameId });

            //  Relacionamento com User
            builder
                .HasOne(ug => ug.user)
                .WithMany(u => u.UsersGames) // precisa existir no User
                .HasForeignKey(ug => ug.UserId);

            //  Relacionamento com Game
            builder
                .HasOne(ug => ug.game)
                .WithMany(g => g.UsersGames) // precisa existir no Game
                .HasForeignKey(ug => ug.GameId);

            //  Valor pago
            builder
                .Property(ug => ug.ValuePay)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Data da compra
            builder
                .Property(ug => ug.PurchaseDate)
                .HasColumnType("datetime2")
                .IsRequired();
        }
    }
}


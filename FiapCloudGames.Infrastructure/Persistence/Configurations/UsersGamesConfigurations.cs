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

            // Sem relacionamento (FK) com User/Game: existência do Game é validada na aplicação antes do insert
            builder
                .Property(ug => ug.UserId)
                .IsRequired();

            builder
                .Property(ug => ug.GameId)
                .IsRequired();

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

            builder
               .Property(ug => ug.Status)
               .HasConversion<int>()
               .IsRequired();
        }
    }
}


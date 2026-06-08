using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class populateOnSalesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "OnSales",
               columns: new[] { "Id", "GameId", "DiscountPercentage", "StartDate", "EndDate", "Status" },
               values: new object[,]
               {
                    {
                        Guid.NewGuid(),
                        Guid.Parse("11111111-1111-1111-1111-111111111111"), // Zelda
                        15m,
                        DateTime.Now,
                        DateTime.Now.AddDays(7),
                        1
                    },
                    {
                        Guid.NewGuid(),
                        Guid.Parse("22222222-2222-2222-2222-222222222222"), // God of War
                        20m,
                        DateTime.Now,
                        DateTime.Now.AddDays(10),
                        1
                    },
                    {
                        Guid.NewGuid(),
                        Guid.Parse("33333333-3333-3333-3333-333333333333"), // Red Dead Redemption 2
                        25m,
                        DateTime.Now,
                        DateTime.Now.AddDays(14),
                        1
                    },
                    {
                        Guid.NewGuid(),
                        Guid.Parse("44444444-4444-4444-4444-444444444444"), // Witcher 3
                        30m,
                        DateTime.Now,
                        DateTime.Now.AddDays(20),
                        4
                    },
                    {
                        Guid.NewGuid(),
                        Guid.Parse("55555555-5555-5555-5555-555555555555"), // Minecraft
                        10m,
                        DateTime.Now,
                        DateTime.Now.AddDays(5),
                        1
                    }
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

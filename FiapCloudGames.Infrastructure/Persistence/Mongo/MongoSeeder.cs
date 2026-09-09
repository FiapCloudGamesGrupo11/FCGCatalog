using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Enums;
using MongoDB.Driver;

namespace FiapCloudGames.Infrastructure.Persistence.Mongo
{
    // Popula as coleções Games e OnSales com os mesmos dados de exemplo usados nas antigas migrations SQL
    public static class MongoSeeder
    {
        public static async Task SeedAsync(MongoContext context)
        {
            var games = context.Database.GetCollection<GameFullData>("Games");
            var sales = context.Database.GetCollection<OnSale>("OnSales");

            if (await games.Find(_ => true).AnyAsync())
                return;

            var zeldaId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var godOfWarId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var reddeadId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var witcherId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var minecraftId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            var gamesData = new List<GameFullData>
            {
                new GameFullData
                {
                    Id = zeldaId,
                    Name = "The Legend of Zelda: Breath of the Wild",
                    Description = "An open-world action-adventure game set in the kingdom of Hyrule.",
                    Category = "Action-Adventure",
                    Developer = "Nintendo",
                    Publisher = "Nintendo",
                    Genres = new List<string> { "Action", "Adventure" },
                    Platforms = new List<string> { "Switch" },
                    Price = 59.99m,
                    ReleaseDate = new DateTime(2017, 3, 3),
                    Rating = "E10+",
                    Tags = new List<string> { "Open World" }
                },
                new GameFullData
                {
                    Id = godOfWarId,
                    Name = "God of War",
                    Description = "An action-adventure game following Kratos and his son Atreus on a journey through Norse mythology.",
                    Category = "Action-Adventure",
                    Developer = "Santa Monica Studio",
                    Publisher = "Sony Interactive Entertainment",
                    Genres = new List<string> { "Action", "Adventure" },
                    Platforms = new List<string> { "PS4", "PS5", "PC" },
                    Price = 49.99m,
                    ReleaseDate = new DateTime(2018, 4, 20),
                    Rating = "M",
                    Tags = new List<string> { "Mythology" }
                },
                new GameFullData
                {
                    Id = reddeadId,
                    Name = "Red Dead Redemption 2",
                    Description = "An open-world action-adventure game set in the American Wild West.",
                    Category = "Action-Adventure",
                    Developer = "Rockstar Games",
                    Publisher = "Rockstar Games",
                    Genres = new List<string> { "Action", "Adventure" },
                    Platforms = new List<string> { "PS4", "Xbox One", "PC" },
                    Price = 39.99m,
                    ReleaseDate = new DateTime(2018, 10, 26),
                    Rating = "M",
                    Tags = new List<string> { "Open World", "Western" }
                },
                new GameFullData
                {
                    Id = witcherId,
                    Name = "The Witcher 3: Wild Hunt",
                    Description = "An open-world RPG following Geralt of Rivia as he hunts monsters and navigates political intrigue.",
                    Category = "RPG",
                    Developer = "CD Projekt Red",
                    Publisher = "CD Projekt",
                    Genres = new List<string> { "RPG" },
                    Platforms = new List<string> { "PS4", "Xbox One", "PC", "Switch" },
                    Price = 29.99m,
                    ReleaseDate = new DateTime(2015, 5, 19),
                    Rating = "M",
                    Tags = new List<string> { "Fantasy" }
                },
                new GameFullData
                {
                    Id = minecraftId,
                    Name = "Minecraft",
                    Description = "A sandbox game that allows players to build and explore virtual worlds made of blocks.",
                    Category = "Sandbox",
                    Developer = "Mojang Studios",
                    Publisher = "Mojang Studios",
                    Genres = new List<string> { "Sandbox", "Survival" },
                    Platforms = new List<string> { "PC", "Mobile", "Console" },
                    Price = 26.95m,
                    ReleaseDate = new DateTime(2011, 11, 18),
                    Rating = "E10+",
                    Tags = new List<string> { "Building" }
                }
            };

            await games.InsertManyAsync(gamesData);

            var salesData = new List<OnSale>
            {
                new OnSale
                {
                    Id = Guid.NewGuid(),
                    GameId = zeldaId,
                    DiscountPercentage = 15m,
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(7),
                    Status = Status.Active
                },
                new OnSale
                {
                    Id = Guid.NewGuid(),
                    GameId = godOfWarId,
                    DiscountPercentage = 20m,
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(10),
                    Status = Status.Active
                },
                new OnSale
                {
                    Id = Guid.NewGuid(),
                    GameId = reddeadId,
                    DiscountPercentage = 25m,
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(14),
                    Status = Status.Active
                }
            };

            await sales.InsertManyAsync(salesData);
        }
    }
}

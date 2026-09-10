using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Application.DTOs.Game.Response
{
    public class GameResponseFull
    {
        public static GameResponseFull FromGameFullData(GameFullData game)
        {
            return new GameResponseFull
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Category = game.Category,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Genres = game.Genres,
                Platforms = game.Platforms,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Rating = game.Rating,
                Tags = game.Tags
            };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public List<string> Genres { get; set; } = new();
        public List<string> Platforms { get; set; } = new();
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Rating { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
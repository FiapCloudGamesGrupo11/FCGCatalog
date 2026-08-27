using DomainGame = FiapCloudGames.Domain.Entity.Game;
using DomainGameFullData = FiapCloudGames.Domain.Entity.GameFullData;

namespace FiapCloudGames.Application.DTOs.Game.Response
{
    public class GameCreatedResponse
    {
        public GameCreatedResponse(Guid id, string name, decimal price, string description, string category)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }

        public static List<GameCreatedResponse> FromGameList(List<DomainGame> games)
        {
            return games
                .Select(game => new GameCreatedResponse(
                    game.Id,
                    game.Name,
                    game.Price,
                    game.Description,
                    game.Category))
                .ToList();
        }

        public static List<GameCreatedResponse> FromGameFullDataList(List<DomainGameFullData> games)
        {
            return games
                .Select(game => new GameCreatedResponse(
                    game.Id,
                    game.Name,
                    game.Price,
                    game.Description,
                    game.Category))
                .ToList();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}

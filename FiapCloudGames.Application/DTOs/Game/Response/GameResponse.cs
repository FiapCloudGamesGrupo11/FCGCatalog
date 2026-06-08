namespace FiapCloudGames.Application.DTOs.Game.Response
{
    public class GameCreatedResponse
    {
        public GameCreatedResponse(string name, decimal price, string description, string category)
        {
            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}

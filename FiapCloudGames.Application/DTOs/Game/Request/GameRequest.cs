using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.DTOs.Game.Request
{
    public class GameRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public List<string> Genres { get; set; } = new List<string>();
        public List<string> Platforms { get; set; } = new List<string>();
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Rating { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }
}

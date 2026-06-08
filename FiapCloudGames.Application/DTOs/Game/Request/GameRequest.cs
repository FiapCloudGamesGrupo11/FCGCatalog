using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.DTOs.Game.Request
{
    public class GameRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}

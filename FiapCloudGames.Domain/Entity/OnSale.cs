using FiapCloudGames.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiapCloudGames.Domain.Entity
{
    public class OnSale
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }

        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [BsonIgnore]
        public Game? Game { get; set; }

        [NotMapped]
        [BsonIgnore]
        public DateTime DataAtual { get; private set; } = DateTime.Now;

        public Status Status { get; set; }

        public decimal GetDiscountedPrice ()
        {
            if (Game == null || Status == Status.Desactivated) return Game.Price;
            return Game.Price - (Game.Price * DiscountPercentage / 100);
        }

        public void UpdateStatus ()
        {
            Status = (DataAtual >= StartDate && DataAtual <= EndDate)
                ? Status.Active
                : Status.Desactivated;
        }
    }
}

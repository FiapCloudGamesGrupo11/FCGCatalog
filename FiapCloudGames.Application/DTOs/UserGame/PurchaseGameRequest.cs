namespace FiapCloudGames.Application.DTOs.UserGame
{
    public class PurchaseGameRequest
    {
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public decimal ValuePay { get; set; }
    }
}
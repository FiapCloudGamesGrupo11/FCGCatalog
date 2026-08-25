namespace FiapCloudGames.Application.DTOs.UserGame
{
    public class PurchaseGameRequest
    {
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public decimal ValuePay { get; set; }
        public string PaymentMethod { get; set; }
        public string? CardNumber { get; set; }
        public string? Cvv { get; set; }
        public string? ExpirationDate { get; set; }
    }
}
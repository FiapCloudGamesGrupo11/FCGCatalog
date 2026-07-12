using FiapCloudGames.Domain.Enums;

namespace FiapCloudGames.Domain.Entity
{
    public class Order
    {
        public Order() { }
        public Order(Guid id, Guid userId, Guid gameId, decimal amount)
        {
            Id = id;
            UserId = userId;
            GameId = gameId;
            Amount = amount;
            PurchaseDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
        }

        public void ApproveStatus()
        {
            Status = OrderStatus.Approved;
        }

        public void RejectedStatus() {
            Status = OrderStatus.Rejected;
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid GameId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public OrderStatus Status { get; private set; }
    }
}
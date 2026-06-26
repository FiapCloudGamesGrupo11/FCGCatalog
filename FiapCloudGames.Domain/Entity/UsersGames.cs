using FiapCloudGames.Domain.Enums;

namespace FiapCloudGames.Domain.Entity
{
    public class UsersGames
    {
        public Guid UserId { get; private set; }
        public User user{ get; private set; }

        public Guid GameId { get; private set; }
        public Game game { get; private set; }

        public DateTime PurchaseDate { get; private set; }
        public decimal ValuePay { get; private set; }

        public Status Status { get; private set; }

        protected UsersGames() { }

        public UsersGames(Guid userid, Guid jogoId, decimal valuePay)
        {
            UserId = userid;
            GameId = jogoId;
            ValuePay = valuePay;
            PurchaseDate = DateTime.UtcNow;
            Status = Status.Pending;
        }   

        public void ActivateStatus()
        {
            Status = Status.Active;
        }

        public void BlockedStatus() {
            Status = Status.Blocked;
        }
    }

}

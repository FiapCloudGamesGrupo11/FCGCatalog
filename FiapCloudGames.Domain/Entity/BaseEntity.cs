namespace FiapCloudGames.Domain.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
    }
}

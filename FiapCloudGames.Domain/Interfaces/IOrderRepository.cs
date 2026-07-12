using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> Create(Order order);
        Task UpdateStatus(Guid userId, Guid gameId, int status);
    }
}
using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Domain.Interfaces
{
    public interface IUserGameRepository
    {
        Task<UsersGames> Create(UsersGames userGame);
        Task UpdateStatusByOrderId(string orderId, int status);
        Task ActivateUserGame(Guid userId, Guid gameId);
        Task UpdateStatus(Guid userId, Guid gameId, int status);
    }
}

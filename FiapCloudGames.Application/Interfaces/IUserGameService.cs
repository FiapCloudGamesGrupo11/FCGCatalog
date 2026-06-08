using FiapCloudGames.Application.Services;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IUserGameService
    {
        Task AddGameToUser(Guid userId, Guid gameId, decimal valuePay);
    }
}

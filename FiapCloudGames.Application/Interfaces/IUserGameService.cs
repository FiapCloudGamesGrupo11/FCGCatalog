using FiapCloudGames.Application.DTOs.Game.Response;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IUserGameService
    {
        Task AddGameToUser(Guid userId, Guid gameId, decimal valuePay);
        Task<List<GameCreatedResponse>> GetGamesByUserId(Guid userId);
    }
}

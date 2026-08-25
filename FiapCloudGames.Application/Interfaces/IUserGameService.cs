using FiapCloudGames.Application.DTOs.Game.Response;
using FiapCloudGames.Application.DTOs.UserGame;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IUserGameService
    {
        Task AddGameToUser(PurchaseGameRequest request);
        Task<List<GameCreatedResponse>> GetGamesByUserId(Guid userId);
    }
}

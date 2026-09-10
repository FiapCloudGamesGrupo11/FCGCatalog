using FiapCloudGames.Application.DTOs.Game.Request;
using FiapCloudGames.Application.DTOs.Game.Response;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IGameService
    {
        Task<GameCreatedResponse> CreateGame(GameRequest gameRequest);
        Task<IEnumerable<GameCreatedResponse>> GetAllAsync();
        Task<GameResponseFull> GetGameById(Guid id);
        Task<GameCreatedResponse> UpdateGame(Guid id, GameRequest request);

    }
}

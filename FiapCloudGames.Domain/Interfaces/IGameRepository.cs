using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Domain.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> AddAsync(Game game);
        Task<Game> GetGameByID(Guid id);
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> UpdateGameAsync(Game game);
    }
}

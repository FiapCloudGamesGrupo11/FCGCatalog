using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Domain.Interfaces
{
    public interface IMongoGameRepository
    {
        Task<GameFullData?> GetByIdAsync(Guid id);
    
        Task<IEnumerable<GameFullData>> GetAllAsync();

        Task AddAsync(GameFullData game);

        Task<GameFullData?> UpdateAsync(GameFullData game);
    }
}
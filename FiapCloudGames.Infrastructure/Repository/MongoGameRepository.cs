using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Persistence.Mongo;
using MongoDB.Driver;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class MongoGameRepository : IMongoGameRepository
    {
        private readonly IMongoCollection<GameFullData> _games;
        public MongoGameRepository(MongoContext context)
        {
            _games = context.Database.GetCollection<GameFullData>("Games");
            
        }
        public async Task AddAsync(GameFullData game)
        {
            await _games.InsertOneAsync(game);
        }

        public async Task<GameFullData?> UpdateAsync(GameFullData game)
        {
            var result = await _games.ReplaceOneAsync(
                existingGame => existingGame.Id == game.Id,
                game);

            return result.MatchedCount == 0 ? null : game;
        }

        public async Task<IEnumerable<GameFullData>> GetAllAsync()
        {
            var projection = Builders<GameFullData>.Projection
                .Include(game => game.Id)
                .Include(game => game.Name)
                .Include(game => game.Description)
                .Include(game => game.Category)
                .Include(game => game.Price);

            return await _games.Find(_ => true)
                .Project<GameFullData>(projection)
                .ToListAsync();
        }

        public async Task<GameFullData?> GetByIdAsync(Guid id)
        {
            return await _games.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
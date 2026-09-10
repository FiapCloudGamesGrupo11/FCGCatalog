using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Persistence.Mongo;
using MongoDB.Driver;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class MongoOnSaleRepository : IOnSaleRepository
    {
        private readonly IMongoCollection<OnSale> _sales;

        public MongoOnSaleRepository(MongoContext context)
        {
            _sales = context.Database.GetCollection<OnSale>("OnSales");
        }

        public async Task<OnSale?> GetByIdAsync(Guid id)
        {
            return await _sales.Find(sale => sale.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OnSale>> GetAllAsync()
        {
            return await _sales.Find(_ => true).ToListAsync();
        }

        public async Task AddAsync(OnSale entity)
        {
            await _sales.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(OnSale entity)
        {
            await _sales.ReplaceOneAsync(
                sale => sale.Id == entity.Id,
                entity);
        }
    }
}
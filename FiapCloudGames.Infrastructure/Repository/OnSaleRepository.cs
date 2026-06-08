using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class OnSaleRepository : IOnSaleRepository
    {
        private readonly ApplicationDbContext _context;

        public OnSaleRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OnSale?> GetByIdAsync (Guid id)
        {
            return await _context.OnSales.Include(x => x.Game)
                                         .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<OnSale>> GetAllAsync ()
        {
            return await _context.OnSales.Include(x => x.Game).ToListAsync();
        }

        public async Task AddAsync (OnSale entity)
        {
            await _context.OnSales.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync (OnSale entity)
        {
            _context.OnSales.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync (Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.OnSales.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure
{
    public class GameRepository : IGameRepository
    {
        private readonly IApplicationDbContext _context;

        public GameRepository (IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Game> AddAsync (Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game> GetGameByID (Guid id)
        {
            return await _context.Games.Include(g => g.OnSales)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllAsync ()
        {
            return await _context.Games.Include(g => g.OnSales).AsNoTracking().ToListAsync();
        }

        public async Task<Game> UpdateGameAsync (Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return game;
        }
    }
}



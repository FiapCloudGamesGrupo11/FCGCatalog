using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Enums;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class UserGameRepository : IUserGameRepository
    {
        private readonly ApplicationDbContext _context;

        public UserGameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UsersGames> Create(UsersGames userGame)
        {
            try
            {
                await _context.UsersGames.AddAsync(userGame);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            return userGame;
        }

        public async Task<List<Game>> GetGamesByUserId(Guid userId)
        {
            var userGames = await _context.UsersGames
                .Where(ug => ug.UserId == userId && ug.Status == Status.Active)
                .Include(ug => ug.game)
                .Select(ug => ug.game)
                .ToListAsync();

            return userGames;
        }

        public async Task UpdateStatusByOrderId(string orderId, int status)
        {
            // orderId simples — busca direto por userId e gameId via PaymentResult
            throw new NotImplementedException("Use UpdateStatus(Guid userId, Guid gameId, int status)");
        }

        // Novo método mais direto:
        public async Task UpdateStatus(Guid userId, Guid gameId, int status)
        {
            var userGame = await _context.UsersGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame is null)
                throw new Exception($"UserGame não encontrado: userId={userId}, gameId={gameId}");

            if (status == 1)
                userGame.ActivateStatus();
            else
                userGame.BlockedStatus();

            await _context.SaveChangesAsync();
        }

        public async Task ActivateUserGame(Guid userId, Guid gameId)
        {
            var userGame = await _context.UsersGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame is null) throw new Exception($"UserGame não encontrado: userId={userId}, gameId={gameId}");

            userGame.ActivateStatus();
            await _context.SaveChangesAsync();
        }
    }
}

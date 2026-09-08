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
        private readonly IMongoGameRepository _mongoGameRepository;

        public UserGameRepository(ApplicationDbContext context, IMongoGameRepository mongoGameRepository)
        {
            _context = context;
            _mongoGameRepository = mongoGameRepository;
        }

        public async Task<UsersGames> Create(UsersGames userGame)
        {
            // Sem FK entre UsersGames e Game: jogo vive no MongoDB, então valida existência lá antes de inserir
            var game = await _mongoGameRepository.GetByIdAsync(userGame.GameId);
            if (game is null)
                throw new Exception($"Jogo não encontrado: gameId={userGame.GameId}");

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

        public async Task<List<GameFullData>> GetGamesByUserId(Guid userId)
        {
            var gameIds = await _context.UsersGames
                .Where(ug => ug.UserId == userId && ug.Status == Status.Active)
                .Select(ug => ug.GameId)
                .ToListAsync();

            // Jogos vivem no MongoDB, então busca os dados completos lá
            var userGames = new List<GameFullData>();
            var remainingIds = new List<Guid>(gameIds);
            foreach (var gameId in gameIds)
            {
                var game = await _mongoGameRepository.GetByIdAsync(gameId);
                if (game is not null)
                {
                    userGames.Add(game);
                    remainingIds.Remove(gameId);
                }
            }

            // Incremento: jogos legados que ainda vivem no SQL (não encontrados no MongoDB)
            if (remainingIds.Count > 0)
            {
                var legacyGames = await _context.Games
                    .Where(g => remainingIds.Contains(g.Id))
                    .ToListAsync();

                userGames.AddRange(legacyGames.Select(g => new GameFullData
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Category = g.Category,
                    Price = g.Price
                }));
            }

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

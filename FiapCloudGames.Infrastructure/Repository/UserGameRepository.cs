using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Persistence;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class UserGameRepository: IUserGameRepository
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            return userGame;
        }
    }
}

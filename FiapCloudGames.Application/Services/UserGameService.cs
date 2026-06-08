using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;

        public UserGameService(IUserGameRepository userGameRepository)
        {
            _userGameRepository = userGameRepository;
        }


        public async Task AddGameToUser(Guid userId, Guid gameId, decimal valuePay)
        {
            var userGame = new Domain.Entity.UsersGames(userId, gameId, valuePay);
            await _userGameRepository.Create(userGame);
        }
    }
}

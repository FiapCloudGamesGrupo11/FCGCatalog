using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Entity.MessageBus;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;
        private readonly IEventBus _eventBus;


        public UserGameService(IUserGameRepository userGameRepository, IEventBus eventBus)
        {
            _userGameRepository = userGameRepository;
            _eventBus = eventBus;
        }

        public async Task AddGameToUser(
        Guid userId,
        Guid gameId,
        decimal valuePay)
        {
            var userGame = new Domain.Entity.UsersGames(
                userId,
                gameId,
                valuePay);

            await _userGameRepository.Create(userGame);

            var orderPlacedEvent = new OrderPlacedEvent(
                userId,
                gameId,
                valuePay,
                DateTime.UtcNow);

            await _eventBus.PublishAsync(orderPlacedEvent);
        }
    }
}

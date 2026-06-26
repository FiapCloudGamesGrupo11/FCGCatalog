using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Entity.MessageBus;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Application.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;
        private readonly IMessagePublisher _publisher;

        private const string _queueName = "order-placed";

        public UserGameService(IUserGameRepository userGameRepository, IMessagePublisher publisher)
        {
            _userGameRepository = userGameRepository;
            _publisher = publisher;
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

            var paymentDetails = new PaymentDetails("Credit", "123456789", "123", "10/30");

            var orderPlacedEvent = new OrderPlacedEvent(
                userId,
                gameId,
                valuePay,
                DateTime.UtcNow,
                paymentDetails);

            var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderPlacedEvent));

            await _publisher.PublishAsync(_queueName, message);
        }

    
    }
}

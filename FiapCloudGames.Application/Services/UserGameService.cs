using FiapCloudGames.Application.DTOs.Game.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Entity.MessageBus;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.MessageBus;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Application.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessagePublisher _publisher;
        private readonly RabbitMqSettings _rabbitmqSettings;

        public UserGameService(IUserGameRepository userGameRepository, IOrderRepository orderRepository, IMessagePublisher publisher,  RabbitMqSettings options)
        {
            _userGameRepository = userGameRepository;
            _orderRepository = orderRepository;
            _publisher = publisher;
            _rabbitmqSettings = options;
        }

        public async Task AddGameToUser(Guid userId, Guid gameId, decimal amount)
        {
            var newOrder = new Order(Guid.NewGuid(), userId, gameId, amount);
            await _orderRepository.Create(newOrder);

            var paymentDetails = new PaymentDetails("Credit", "123456789", "123", "10/30");
            var orderPlacedEvent = new OrderPlacedEvent(
                newOrder.Id,
                userId,
                gameId,
                amount,
                DateTime.UtcNow,
                paymentDetails);

            var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderPlacedEvent));

            await _publisher.PublishAsync(_rabbitmqSettings.OrderQueueName, message);
        }

        public async Task<List<GameCreatedResponse>> GetGamesByUserId(Guid userId)
        {
            var result = await _userGameRepository.GetGamesByUserId(userId);
            
            var resultMaped = GameCreatedResponse.FromGameList(result);

            return resultMaped;
        }
    }
}

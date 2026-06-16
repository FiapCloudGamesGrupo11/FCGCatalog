using FiapCloudGames.Domain.Interfaces;
using RabbitMQ.Client;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqPublisher(
            IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishAsync(
            string queue,
            byte[] message)
        {
            var channel = await _connection.GetChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queue,
                body: message);
        }
    }
}

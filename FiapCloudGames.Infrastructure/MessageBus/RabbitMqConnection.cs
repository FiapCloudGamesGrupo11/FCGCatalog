using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqConnection(RabbitMqSettings settings)
        {
            _factory = new ConnectionFactory
            {
                HostName = settings.Host,
                UserName = settings.User,
                Password = settings.Password
            };
        }

        public async Task<IChannel> GetChannelAsync()
        {
            var connection =
                await _factory.CreateConnectionAsync();

            return await connection.CreateChannelAsync();
        }
    }
}

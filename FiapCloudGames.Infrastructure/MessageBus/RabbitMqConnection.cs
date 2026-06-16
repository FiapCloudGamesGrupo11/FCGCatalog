using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqConnection()
        {

            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
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

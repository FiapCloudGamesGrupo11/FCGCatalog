using RabbitMQ.Client;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public interface IRabbitMqConnection
    {
        Task<IChannel> GetChannelAsync();
    }
}

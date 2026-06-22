namespace FiapCloudGames.Infrastructure.MessageBus
{
    public interface IRabbitMqConsumer
    {
        Task ConsumeAsync(string queue);
    }
}

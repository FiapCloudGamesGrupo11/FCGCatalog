using FiapCloudGames.Application.Interfaces;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class MessageBusService : IEventBus
    {
        public Task PublishAsync<T>(T @event)
        {
            Console.WriteLine(
                $"[EVENT PUBLISHED] {typeof(T).Name}");

            return Task.CompletedTask;
        }
    }
}

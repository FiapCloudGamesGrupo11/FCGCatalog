namespace FiapCloudGames.Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event);
    }
}

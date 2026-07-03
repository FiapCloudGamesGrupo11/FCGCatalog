namespace FiapCloudGames.Domain.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync(
            string queue,
            byte[] message);
    }
}



namespace FiapCloudGames.Domain.Entity.MessageBus
{
    public record OrderPlacedEvent(
        string OrderId,
        Guid UserId,
        Guid GameId,
        decimal Price,
        DateTime CreatedAt,
        PaymentDetails PaymentDetails
    );
}

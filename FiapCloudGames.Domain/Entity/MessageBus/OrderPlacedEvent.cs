

namespace FiapCloudGames.Domain.Entity.MessageBus
{
    public record OrderPlacedEvent(
        Guid OrderId,
        Guid UserId,
        Guid GameId,
        decimal Amount,
        DateTime CreatedAt,
        PaymentDetails PaymentDetails
    );
}

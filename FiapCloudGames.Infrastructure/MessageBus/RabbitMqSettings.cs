namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqSettings
    {
        public string Host { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string OrderQueueName { get; set; } = string.Empty;
        public string PaymentProcessQueueName { get; set; } = string.Empty;
    }
}

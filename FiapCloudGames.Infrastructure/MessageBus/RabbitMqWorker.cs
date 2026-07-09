using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqWorker : BackgroundService
    {
        private readonly IRabbitMqConsumer _consumer;
        private readonly RabbitMqSettings _rabbitmqSettings;

        public RabbitMqWorker(IRabbitMqConsumer consumer, IOptions<RabbitMqSettings> options)
        {
            _consumer = consumer;
            _rabbitmqSettings = options.Value;
        }


        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            await _consumer.ConsumeAsync(_rabbitmqSettings.PaymentProcessQueueName);
        }
    }
}

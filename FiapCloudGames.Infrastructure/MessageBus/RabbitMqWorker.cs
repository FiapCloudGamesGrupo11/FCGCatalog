using Microsoft.Extensions.Hosting;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqWorker : BackgroundService
    {
        private readonly IRabbitMqConsumer _consumer;


        public RabbitMqWorker(IRabbitMqConsumer consumer)
        {
            _consumer = consumer;
        }


        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            await _consumer.ConsumeAsync("payment-processed");
        }
    }
}

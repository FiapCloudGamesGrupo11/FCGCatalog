using FiapCloudGames.Domain.Interfaces;
using RabbitMQ.Client;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqPublisher(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishAsync(string queue, byte[] message)
        {
            var channel = await _connection.GetChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            var headers = new Dictionary<string, object?>();

            NewRelic.Api.Agent.NewRelic.GetAgent()
                .CurrentTransaction
                .InsertDistributedTraceHeaders(
                    headers,
                    (carrier, key, value) => carrier[key] = value);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                Headers = headers
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queue,
                mandatory: false,
                basicProperties: properties,
                body: message);

            Console.WriteLine(
                $"Mensagem publicada na fila '{queue}': {System.Text.Encoding.UTF8.GetString(message)}");
        }
    }
}
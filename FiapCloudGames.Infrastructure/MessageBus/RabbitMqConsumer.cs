using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqConsumer : IRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqConsumer(IRabbitMqConnection connection, IServiceScopeFactory scopeFactory)
        {
            _connection = connection;
            _scopeFactory = scopeFactory;
        }

        public async Task ConsumeAsync(string queue)
        {
            var channel = await _connection.GetChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var result = JsonSerializer.Deserialize<PaymentResult>(message);

                    if (result is null)
                    {
                        Console.WriteLine("Mensagem inválida recebida, descartando.");
                        await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: false);
                        return;
                    }

                    using var scope = _scopeFactory.CreateScope();

                    Console.WriteLine($"Mensagem recebida: {message}");

                    var userGameRepository = scope.ServiceProvider.GetRequiredService<IUserGameRepository>();

                    bool isApproved = result.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase);
                    int newStatus = isApproved ? 1 : 2;

                    await userGameRepository.UpdateStatus(result.UserId, result.GameId, newStatus);

                    if (isApproved)
                        await userGameRepository.ActivateUserGame(result.UserId, result.GameId);

                    await channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                    await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: false);
                }
            };

            await channel.BasicConsumeAsync(
                queue: queue,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
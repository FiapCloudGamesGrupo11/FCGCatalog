using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FiapCloudGames.Infrastructure.MessageBus
{
    public class RabbitMqConsumer : IRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqConsumer(IRabbitMqConnection connection)
        {
            _connection = connection;
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


                Console.WriteLine($"Mensagem recebida: {message}");


                // aqui você chama sua Application Service


                await channel.BasicAckAsync(
                    deliveryTag: args.DeliveryTag,
                    multiple: false
                );
            };


            await channel.BasicConsumeAsync(
                queue: queue,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}

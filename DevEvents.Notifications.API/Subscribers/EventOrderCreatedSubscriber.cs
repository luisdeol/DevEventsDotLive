
using DevEvents.Notifications.API.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DevEvents.Notifications.API.Subscribers
{
    public class EventOrderCreatedSubscriber : IHostedService
    {
        private readonly IModel _channel;
        private const string QUEUE = "order-created";
        private readonly IServiceProvider _provider;

        public EventOrderCreatedSubscriber(IServiceProvider provider)
        {
            _provider = provider;

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection("consumer-dotlive-rabbitmq");

            _channel = connection.CreateModel();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var array = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(array);

                var message = JsonSerializer.Deserialize<EventOrderCompleted>(contentString);

                await SendEmail(message);

                Console.WriteLine($"Mensagem recebida: {contentString}");

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE, false, consumer);
        }

        public async Task SendEmail(EventOrderCompleted @event)
        {
            using (var scope = _provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<INotificationService>();

                await service.SendEmail(@event.Email, "order-created", new Dictionary<string, string>
                {
                    {  "name",@event.FullName }
                });
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class EventOrderCompleted
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public decimal Price { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}

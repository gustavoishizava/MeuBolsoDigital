using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class TransactionValueChangedIntegrationEventHandler : INotificationHandler<TransactionValueChangedIntegrationEvent>
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private readonly ILogger<TransactionValueChangedIntegrationEventHandler> _logger;

        public TransactionValueChangedIntegrationEventHandler(ILogger<TransactionValueChangedIntegrationEventHandler> logger)
        {
            _queueName = nameof(TransactionValueChangedIntegrationEvent);
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            _logger = logger;
        }

        public Task Handle(TransactionValueChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--------- Integration Event: Mudança no valor da transação -------------");

            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var stringfiedMessage = JsonSerializer.Serialize(notification, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var messageBytes = Encoding.UTF8.GetBytes(stringfiedMessage);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _queueName,
                basicProperties: null,
                body: messageBytes);

            return Task.CompletedTask;
        }
    }
}
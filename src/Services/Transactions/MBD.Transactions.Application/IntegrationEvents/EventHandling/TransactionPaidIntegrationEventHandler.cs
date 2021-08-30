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
    public class TransactionPaidIntegrationEventHandler : INotificationHandler<TransactionPaidIntegrationEvent>
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private readonly ILogger<TransactionPaidIntegrationEventHandler> _logger;

        public TransactionPaidIntegrationEventHandler(ILogger<TransactionPaidIntegrationEventHandler> logger)
        {
            _queueName = nameof(TransactionPaidIntegrationEvent);
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            _logger = logger;
        }

        public Task Handle(TransactionPaidIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("---------- Ingration Event: Transação paga ------------");

            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var stringfiedMessage = JsonSerializer.Serialize(notification, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _queueName,
                basicProperties: null,
                body: bytesMessage);

            return Task.CompletedTask;
        }
    }
}
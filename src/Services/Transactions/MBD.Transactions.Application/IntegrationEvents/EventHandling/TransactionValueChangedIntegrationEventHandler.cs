using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using MessageBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class TransactionValueChangedIntegrationEventHandler : INotificationHandler<TransactionValueChangedIntegrationEvent>
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private readonly ILogger<TransactionValueChangedIntegrationEventHandler> _logger;

        public TransactionValueChangedIntegrationEventHandler(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger<TransactionValueChangedIntegrationEventHandler> logger)
        {
            _queueName = nameof(TransactionValueChangedIntegrationEvent);
            var rabbitMqConfiguration = rabbitMqOptions.Value;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = rabbitMqConfiguration.HostName,
                UserName = rabbitMqConfiguration.UserName,
                Password = rabbitMqConfiguration.Password
            };
            _logger = logger;
        }

        public Task Handle(TransactionValueChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--------- Integration Event: Mudança no valor da transação -------------");

            return Task.CompletedTask;
        }
    }
}
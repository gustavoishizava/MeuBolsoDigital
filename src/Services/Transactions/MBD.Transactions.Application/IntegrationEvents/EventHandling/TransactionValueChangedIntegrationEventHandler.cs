using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class TransactionValueChangedIntegrationEventHandler : INotificationHandler<TransactionValueChangedIntegrationEvent>
    {
        private readonly ILogger<TransactionValueChangedIntegrationEventHandler> _logger;

        public TransactionValueChangedIntegrationEventHandler(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger<TransactionValueChangedIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(TransactionValueChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--------- Integration Event: Mudança no valor da transação -------------");

            return Task.CompletedTask;
        }
    }
}
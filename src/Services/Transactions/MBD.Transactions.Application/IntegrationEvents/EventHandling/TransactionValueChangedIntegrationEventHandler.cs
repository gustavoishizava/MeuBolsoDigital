using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class TransactionValueChangedIntegrationEventHandler : INotificationHandler<TransactionValueChangedIntegrationEvent>
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TransactionValueChangedIntegrationEventHandler> _logger;

        public TransactionValueChangedIntegrationEventHandler(IMessageBus messageBus, ILogger<TransactionValueChangedIntegrationEventHandler> logger)
        {
            _messageBus = messageBus;
            _logger = logger;
        }

        public Task Handle(TransactionValueChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--------- Integration Event: Mudança no valor da transação -------------");

            _messageBus.Publish<TransactionValueChangedIntegrationEvent>(notification);

            return Task.CompletedTask;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class TransactionPaidIntegrationEventHandler : INotificationHandler<TransactionPaidIntegrationEvent>
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TransactionPaidIntegrationEventHandler> _logger;

        public TransactionPaidIntegrationEventHandler(IMessageBus messageBus, ILogger<TransactionPaidIntegrationEventHandler> logger)
        {
            _messageBus = messageBus;
            _logger = logger;
        }

        public Task Handle(TransactionPaidIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("============= Ingration Event: Transação paga =============");

            _messageBus.Publish<TransactionPaidIntegrationEvent>(notification);

            return Task.CompletedTask;
        }
    }
}
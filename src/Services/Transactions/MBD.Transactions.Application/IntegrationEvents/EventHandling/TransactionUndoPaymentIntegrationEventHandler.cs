using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class TransactionUndoPaymentIntegrationEventHandler : INotificationHandler<TransactionUndoPaymentIntegrationEvent>
    {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TransactionUndoPaymentIntegrationEventHandler> _logger;

        public TransactionUndoPaymentIntegrationEventHandler(IMessageBus messageBus, ILogger<TransactionUndoPaymentIntegrationEventHandler> logger)
        {
            _messageBus = messageBus;
            _logger = logger;
        }

        public Task Handle(TransactionUndoPaymentIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--------- Integration Event: Pagamento desfeito -------------");

            _messageBus.Publish<TransactionUndoPaymentIntegrationEvent>(notification);

            return Task.CompletedTask;
        }
    }
}
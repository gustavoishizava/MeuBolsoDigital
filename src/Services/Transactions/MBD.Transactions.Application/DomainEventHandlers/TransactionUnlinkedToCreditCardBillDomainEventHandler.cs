using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class TransactionUnlinkedToCreditCardBillDomainEventHandler : INotificationHandler<UnlinkedToCreditCardBillDomainEvent>
    {
        private readonly IMessageBus _messageBus;

        public TransactionUnlinkedToCreditCardBillDomainEventHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Task Handle(UnlinkedToCreditCardBillDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new TransactionUnlinkedToCreditCardBillIntegrationEvent(
                notification.Id,
                notification.BankAccountId,
                notification.CreditCardBillId);

            _messageBus.Publish(integrationEvent);

            return Task.CompletedTask;
        }
    }
}
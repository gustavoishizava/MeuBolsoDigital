using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class LinkedToCreditCardBillDomainEventHandler : INotificationHandler<LinkedToCreditCardBillDomainEvent>
    {
        private readonly IMessageBus _messageBus;

        public LinkedToCreditCardBillDomainEventHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Task Handle(LinkedToCreditCardBillDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new TransactionLinkedToCreditCardBillIntegrationEvent(
                notification.Id,
                notification.BankAccountId,
                notification.CreditCardBillId,
                notification.CreatedAt,
                notification.Value);

            _messageBus.Publish(integrationEvent);

            return Task.CompletedTask;
        }
    }
}
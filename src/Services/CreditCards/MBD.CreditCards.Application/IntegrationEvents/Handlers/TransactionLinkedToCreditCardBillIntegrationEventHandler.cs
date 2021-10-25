using System;
using System.Threading;
using System.Threading.Tasks;
using MBD.CreditCards.Application.IntegrationEvents.Events;
using MediatR;

namespace MBD.CreditCards.Application.IntegrationEvents.Handlers
{
    public class TransactionLinkedToCreditCardBillIntegrationEventHandler : INotificationHandler<TransactionLinkedToCreditCardBillIntegrationEvent>
    {
        public Task Handle(TransactionLinkedToCreditCardBillIntegrationEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
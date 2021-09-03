using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class RealizedPaymentDomainEventHandler : INotificationHandler<RealizedPaymentDomainEvent>
    {
        private readonly IMediator _mediator;

        public RealizedPaymentDomainEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(RealizedPaymentDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new TransactionPaidIntegrationEvent(notification.Id, notification.Value, notification.Date, notification.BankAccountId, notification.Type);
            _mediator.Publish(integrationEvent);

            return Task.CompletedTask;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class ReversedPaymentDomainEventHandler : INotificationHandler<ReversedPaymentDomainEvent>
    {
        private readonly IMediator _mediator;

        public ReversedPaymentDomainEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(ReversedPaymentDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new TransactionUndoPaymentIntegrationEvent(notification.Id, notification.Value);
            _mediator.Publish(integrationEvent);

            return Task.CompletedTask;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class ValueChangedDomainEventHandler : INotificationHandler<ValueChangedDomainEvent>
    {
        private readonly IMediator _mediator;

        public ValueChangedDomainEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(ValueChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new TransactionValueChangedIntegrationEvent(notification.Id, notification.NewValue, notification.OldValue));
        }
    }
}
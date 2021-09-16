using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Application.IntegrationEvents;
using MBD.BankAccounts.Domain.Events;
using MediatR;

namespace MBD.BankAccounts.Application.DomainEvents
{
    public class DescriptionChangedDomainEventHandler : INotificationHandler<DescriptionChangedDomainEvent>
    {
        private readonly IMediator _mediator;

        public DescriptionChangedDomainEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(DescriptionChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            _mediator.Publish(new BankAccountDescriptionChangedIntegrationEvent(notification.Id, notification.OldDescription, notification.NewDescription));

            return Task.CompletedTask;
        }
    }
}
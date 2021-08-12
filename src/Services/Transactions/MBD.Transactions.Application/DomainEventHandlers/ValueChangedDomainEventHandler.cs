using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class ValueChangedDomainEventHandler : INotificationHandler<ValueChangedDomainEvent>
    {
        public Task Handle(ValueChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Lançar evento de integração
            return Task.CompletedTask;
        }
    }
}
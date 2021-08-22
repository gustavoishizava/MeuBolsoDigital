using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class CategoryCreatedDomainEventHandler : INotificationHandler<CategoryCreatedDomainEvent>
    {
        public Task Handle(CategoryCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
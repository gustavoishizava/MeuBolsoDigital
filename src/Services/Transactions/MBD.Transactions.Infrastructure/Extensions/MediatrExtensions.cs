using System.Linq;
using System.Threading.Tasks;
using MBD.Core.Extensions;
using MBD.Transactions.Domain.Entities.Common;
using MBD.Transactions.Infrastructure.Context;
using MediatR;

namespace MBD.Transactions.Infrastructure.Extensions
{
    public static class TransactionDbContextExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, TransactionContext context)
        {
            var entities = context.ChangeTracker.Entries<BaseEntityWithEvent>()
                .Where(x => !x.Entity.Events.IsNullOrEmpty());

            var domainEvents = entities.SelectMany(x => x.Entity.Events).ToList();

            entities.ToList().ForEach(x => x.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async (domainEvent) => await mediator.Publish(domainEvent));

            await Task.WhenAll(tasks);
        }
    }
}
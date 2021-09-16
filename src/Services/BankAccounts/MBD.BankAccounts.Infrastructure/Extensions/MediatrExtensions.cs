using System.Linq;
using System.Threading.Tasks;
using MBD.BankAccounts.Domain.Entities.Common;
using MBD.BankAccounts.Infrastructure.Context;
using MBD.Core.Extensions;
using MediatR;

namespace MBD.BankAccounts.Infrastructure.Extensions
{
    public static class MediatrExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, BankAccountContext context)
        {
            var entities = context.ChangeTracker.Entries<BaseEntityWithEvent>()
                .Where(x => !x.Entity.Events.IsNullOrEmpty());

            var domainEvents = entities.SelectMany(x => x.Entity.Events).OrderBy(x => x.TimeStamp).ToList();

            entities.ToList().ForEach(x => x.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async (domainEvent) => await mediator.Publish(domainEvent));

            await Task.WhenAll(tasks);
        }
    }
}
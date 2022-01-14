using System.Threading;
using System.Threading.Tasks;
using MBD.IntegrationEventLog.Services;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class ValueChangedDomainEventHandler : INotificationHandler<ValueChangedDomainEvent>
    {
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public ValueChangedDomainEventHandler(IIntegrationEventLogService integrationEventLogService)
        {
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task Handle(ValueChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _integrationEventLogService
                .SaveEventAsync(new TransactionValueChangedIntegrationEvent(
                    notification.Id, notification.NewValue, notification.OldValue), "value_changed");
        }
    }
}
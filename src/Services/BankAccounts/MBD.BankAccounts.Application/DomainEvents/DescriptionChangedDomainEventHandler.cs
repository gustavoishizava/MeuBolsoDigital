using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Application.IntegrationEvents;
using MBD.BankAccounts.Domain.Events;
using MBD.IntegrationEventLog.Services;
using MediatR;

namespace MBD.BankAccounts.Application.DomainEvents
{
    public class DescriptionChangedDomainEventHandler : INotificationHandler<DescriptionChangedDomainEvent>
    {
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public DescriptionChangedDomainEventHandler(IIntegrationEventLogService integrationEventLogService)
        {
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task Handle(DescriptionChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _integrationEventLogService
                .SaveEventAsync(new BankAccountDescriptionChangedIntegrationEvent(
                    notification.Id, notification.OldDescription, notification.NewDescription));
        }
    }
}
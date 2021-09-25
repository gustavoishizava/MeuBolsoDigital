using System.Threading;
using System.Threading.Tasks;
using MBD.IntegrationEventLog.Services;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class RealizedPaymentDomainEventHandler : INotificationHandler<RealizedPaymentDomainEvent>
    {
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public RealizedPaymentDomainEventHandler(IIntegrationEventLogService integrationEventLogService)
        {
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task Handle(RealizedPaymentDomainEvent notification, CancellationToken cancellationToken)
        {
            await _integrationEventLogService
                .SaveEventAsync(new TransactionPaidIntegrationEvent(
                    notification.Id, notification.Value, notification.Date, notification.BankAccountId, notification.Type));
        }
    }
}
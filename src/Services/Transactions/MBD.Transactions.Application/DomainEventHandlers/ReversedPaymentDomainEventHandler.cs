using System.Threading;
using System.Threading.Tasks;
using MBD.IntegrationEventLog.Services;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Events;
using MediatR;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class ReversedPaymentDomainEventHandler : INotificationHandler<ReversedPaymentDomainEvent>
    {
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public ReversedPaymentDomainEventHandler(IIntegrationEventLogService integrationEventLogService)
        {
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task Handle(ReversedPaymentDomainEvent notification, CancellationToken cancellationToken)
        {
            await _integrationEventLogService
                .SaveEventAsync(new TransactionUndoPaymentIntegrationEvent(notification.Id));
        }
    }
}
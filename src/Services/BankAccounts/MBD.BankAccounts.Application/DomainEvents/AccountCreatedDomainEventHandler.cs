using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Application.IntegrationEvents;
using MBD.BankAccounts.Domain.Events;
using MBD.IntegrationEventLog.Services;
using MediatR;

namespace MBD.BankAccounts.Application.DomainEvents
{
    public class AccountCreatedDomainEventHandler : INotificationHandler<AccountCreatedDomainEvent>
    {
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public AccountCreatedDomainEventHandler(IIntegrationEventLogService service)
        {
            _integrationEventLogService = service;
        }

        public async Task Handle(AccountCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _integrationEventLogService.SaveEventAsync(new BankAccountCreatedIntegrationEvent(
                notification.Id,
                notification.TenantId,
                notification.Description,
                notification.Type));
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Infrastructure.Context;
using MBD.IntegrationEventLog.Services;
using MediatR;

namespace MBD.BankAccounts.Application.IntegrationEvents
{
    public class BankAccountDescriptionChangedIntegrationEventHandler : INotificationHandler<BankAccountDescriptionChangedIntegrationEvent>
    {
        private readonly IIntegrationEventLogService _integrationEventLogsService;
        private readonly BankAccountContext _context;

        public BankAccountDescriptionChangedIntegrationEventHandler(IIntegrationEventLogService integrationEventLogsService, BankAccountContext context)
        {
            _integrationEventLogsService = integrationEventLogsService;
            _context = context;
        }

        public Task Handle(BankAccountDescriptionChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _integrationEventLogsService
                .SaveEventAsync<BankAccountDescriptionChangedIntegrationEvent>(notification);

            return Task.CompletedTask;
        }
    }
}
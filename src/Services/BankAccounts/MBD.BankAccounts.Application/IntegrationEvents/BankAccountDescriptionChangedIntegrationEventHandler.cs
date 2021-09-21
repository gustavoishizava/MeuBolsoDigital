using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Infrastructure.Context;
using MBD.IntegrationEventLog.Services;
using MBD.MessageBus;
using MediatR;

namespace MBD.BankAccounts.Application.IntegrationEvents
{
    public class BankAccountDescriptionChangedIntegrationEventHandler : INotificationHandler<BankAccountDescriptionChangedIntegrationEvent>
    {
        private readonly IMessageBus _messageBus;
        private readonly IIntegrationEventLogService _integrationEventLogsService;
        private readonly BankAccountContext _context;

        public BankAccountDescriptionChangedIntegrationEventHandler(IMessageBus messageBus, IIntegrationEventLogService integrationEventLogsService, BankAccountContext context)
        {
            _messageBus = messageBus;
            _integrationEventLogsService = integrationEventLogsService;
            _context = context;
        }

        public Task Handle(BankAccountDescriptionChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _integrationEventLogsService
                .SaveEventAsync<BankAccountDescriptionChangedIntegrationEvent>(notification);

            // REMOVER PUBLICACAO E PASSAR PARA OUTBOX
            _messageBus.Publish<BankAccountDescriptionChangedIntegrationEvent>(notification);

            return Task.CompletedTask;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MBD.MessageBus;
using MediatR;

namespace MBD.BankAccounts.Application.IntegrationEvents
{
    public class BankAccountDescriptionChangedIntegrationEventHandler : INotificationHandler<BankAccountDescriptionChangedIntegrationEvent>
    {
        private readonly IMessageBus _messageBus;

        public BankAccountDescriptionChangedIntegrationEventHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public Task Handle(BankAccountDescriptionChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            _messageBus.Publish<BankAccountDescriptionChangedIntegrationEvent>(notification);

            return Task.CompletedTask;
        }
    }
}
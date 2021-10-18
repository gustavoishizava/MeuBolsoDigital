using System.Threading;
using System.Threading.Tasks;
using MBD.IntegrationEventLog.Services;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Response.Models;
using MBD.Transactions.Domain.Events;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class RealizedPaymentDomainEventHandler : INotificationHandler<RealizedPaymentDomainEvent>
    {
        private readonly IIntegrationEventLogService _integrationEventLogService;
        private readonly IMongoCollection<TransactionModel> _transactions;

        public RealizedPaymentDomainEventHandler(IIntegrationEventLogService integrationEventLogService, ITransactionDatabaseSettings settings)
        {
            _integrationEventLogService = integrationEventLogService;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
        }

        public async Task Handle(RealizedPaymentDomainEvent notification, CancellationToken cancellationToken)
        {
            await _integrationEventLogService
                .SaveEventAsync(new TransactionPaidIntegrationEvent(
                    notification.Id, notification.Value, notification.Date, notification.BankAccountId, notification.Type));
        }
    }
}
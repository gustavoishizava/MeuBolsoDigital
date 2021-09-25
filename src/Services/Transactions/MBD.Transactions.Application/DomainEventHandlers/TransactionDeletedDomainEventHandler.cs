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
    public class TransactionDeletedDomainEventHandler : INotificationHandler<TransactionDeletedDomainEvent>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public TransactionDeletedDomainEventHandler(ITransactionDatabaseSettings settings, IIntegrationEventLogService integrationEventLogService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task Handle(TransactionDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            _transactions.DeleteOne(x => x.Id == notification.TransactionId.ToString());
            await _integrationEventLogService
                .SaveEventAsync(new TransactionUndoPaymentIntegrationEvent(notification.TransactionId));
        }
    }
}
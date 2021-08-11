using System.Threading;
using System.Threading.Tasks;
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

        public TransactionDeletedDomainEventHandler(ITransactionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
        }

        public Task Handle(TransactionDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            _transactions.DeleteOne(x => x.Id == notification.TransactionId.ToString());
            return Task.CompletedTask;
        }
    }
}
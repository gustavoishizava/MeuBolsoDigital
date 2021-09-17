using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Response.Models;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class BankAccountDescriptionIntegrationEventHandler : INotificationHandler<BankAccountDescriptionChangedIntegrationEvent>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;

        public BankAccountDescriptionIntegrationEventHandler(ITransactionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
        }

        public async Task Handle(BankAccountDescriptionChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var filter = Builders<TransactionModel>.Filter.Where(x => x.BankAccount.Id == notification.Id.ToString());
            var update = Builders<TransactionModel>.Update.Set(x => x.BankAccount.Description, notification.NewDescription);

            await _transactions.UpdateManyAsync(filter, update);
        }
    }
}
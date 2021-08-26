using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Response.Models;
using MBD.Transactions.Domain.Events;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class CategoryNameChangedDomainEventHandler : INotificationHandler<CategoryNameChangedDomainEvent>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;

        public CategoryNameChangedDomainEventHandler(ITransactionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
        }

        public async Task Handle(CategoryNameChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var filter = Builders<TransactionModel>.Filter.Where(x => x.Category.Id == notification.Id.ToString());
            var update = Builders<TransactionModel>.Update.Set(x => x.Category.Name, notification.NewName);

            await _transactions.UpdateManyAsync(filter, update);
        }
    }
}
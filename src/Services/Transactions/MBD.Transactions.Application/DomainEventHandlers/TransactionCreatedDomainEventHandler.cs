using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Response.Models;
using MBD.Transactions.Domain.Events;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class TransactionCreatedDomainEventHandler : INotificationHandler<TransactionCreatedDomainEvent>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;

        public TransactionCreatedDomainEventHandler(ITransactionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
        }
        
        public async Task Handle(TransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var transactionModel = new TransactionModel(
                notification.AggregateId,
                notification.TenantId,
                new BankAccountModel(notification.BankAccountId, notification.BankAccountDescription),
                new CategoryModel(notification.CategoryId, notification.CategoryName),
                notification.ReferenceDate,
                notification.DueDate,
                notification.PaymentDate,
                notification.Status,
                notification.Value,
                notification.Description
            );

            await _transactions.InsertOneAsync(transactionModel);
        }
    }
}
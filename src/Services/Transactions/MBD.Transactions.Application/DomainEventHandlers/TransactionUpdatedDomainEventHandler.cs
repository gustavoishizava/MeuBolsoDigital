using System.Threading;
using System.Threading.Tasks;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Response.Models;
using MBD.Transactions.Domain.Events;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.DomainEventHandlers
{
    public class TransactionUpdatedDomainEventHandler : INotificationHandler<TransactionUpdatedDomainEvent>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;

        public TransactionUpdatedDomainEventHandler(ITransactionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
        }

        public async Task Handle(TransactionUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var transactionTask = await _transactions.FindAsync(x => x.Id == notification.AggregateId.ToString());
            var transaction = transactionTask.FirstOrDefault();
            transaction.BankAccount = new BankAccountModel(notification.BankAccountId, notification.BankAccountDescription);
            transaction.Category = new CategoryModel(notification.CategoryId, notification.CategoryName);
            transaction.ReferenceDate = notification.ReferenceDate;
            transaction.DueDate = notification.DueDate;
            transaction.PaymentDate = notification.PaymentDate;
            transaction.Status = notification.Status;
            transaction.Value = notification.Value;
            transaction.Description = notification.Description;

            _transactions.ReplaceOne(x => x.Id == notification.AggregateId.ToString(), transaction);
        }
    }
}
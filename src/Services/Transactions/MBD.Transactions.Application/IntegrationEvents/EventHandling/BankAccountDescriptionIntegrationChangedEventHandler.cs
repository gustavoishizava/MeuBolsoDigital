using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Response.Models;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class BankAccountDescriptionChangedIntegrationEventHandler : INotificationHandler<BankAccountDescriptionChangedIntegrationEvent>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountDescriptionChangedIntegrationEventHandler(ITransactionDatabaseSettings settings, IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(BankAccountDescriptionChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(notification.Id);
            if (bankAccount is null)
                return;

            bankAccount.SetDescription(notification.NewDescription);
            var filter = Builders<TransactionModel>.Filter.Where(x => x.BankAccount.Id == notification.Id.ToString());
            var update = Builders<TransactionModel>.Update.Set(x => x.BankAccount.Description, notification.NewDescription);

            await _unitOfWork.SaveChangesAsync();
            await _transactions.UpdateManyAsync(filter, update);
        }
    }
}
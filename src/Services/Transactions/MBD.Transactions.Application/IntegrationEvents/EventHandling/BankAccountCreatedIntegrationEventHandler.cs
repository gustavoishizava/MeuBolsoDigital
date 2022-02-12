using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.Transactions.Application.IntegrationEvents.Events;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.IntegrationEvents.EventHandling
{
    public class BankAccountCreatedIntegrationEventHandler : INotificationHandler<BankAccountCreatedIntegrationEvent>
    {
        private readonly IBankAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountCreatedIntegrationEventHandler(IBankAccountRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(BankAccountCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var exists = await _repository.GetByIdAsync(notification.Id);
            if (exists is not null)
                return;

            var bankAccount = new BankAccount(notification.Id, notification.TenantId, notification.Description);
            _repository.Add(bankAccount);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
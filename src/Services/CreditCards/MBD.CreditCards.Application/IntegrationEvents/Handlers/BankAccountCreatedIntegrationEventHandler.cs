using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.CreditCards.Application.IntegrationEvents.Events;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.CreditCards.Application.IntegrationEvents.Handlers
{
    public class BankAccountCreatedIntegrationEventHandler : INotificationHandler<BankAccountCreatedIntegrationEvent>
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountCreatedIntegrationEventHandler(IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork)
        {
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(BankAccountCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var bankAccount = new BankAccount(notification.Id, notification.TenantId, notification.Description);
            _bankAccountRepository.Add(bankAccount);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
using System;
using System.Threading.Tasks;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.BankAccounts.Domain.Interfaces.Repositories;
using MBD.BankAccounts.Domain.Interfaces.Services;
using MBD.Core.Data;
using MBD.Core.DomainObjects;

namespace MBD.BankAccounts.Domain.Services
{
    public class TransactionManagementService : ITransactionManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;

        public TransactionManagementService(IUnitOfWork unitOfWork, IAccountRepository accountRepository)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        public async Task AddTransactionToAccountAsync(Guid accountId, Guid transactionId, decimal value, TransactionType type, DateTime createdAt)
        {
            var account = await _accountRepository.GetByIdAsync(accountId, true);
            if (account == null)
                throw new DomainException($"Nenhuma conta encontrada com o Id='{accountId}'.");

            account.AddTransaction(transactionId, createdAt, value, type);
            _accountRepository.Update(account);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
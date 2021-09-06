using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.BankAccounts.Domain.Entities;
using MBD.Core.Data;

namespace MBD.BankAccounts.Domain.Interfaces.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(Guid id, bool ignoreGlobalFilter);
        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        void RemoveTransaction(Transaction transaction);
    }
}
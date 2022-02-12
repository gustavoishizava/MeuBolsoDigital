using System;
using System.Threading.Tasks;
using MBD.Transactions.Domain.Entities;

namespace MBD.Transactions.Domain.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetByIdAsync(Guid id);
        void Add(BankAccount bankAccount);
        void Update(BankAccount bankAccount);
    }
}
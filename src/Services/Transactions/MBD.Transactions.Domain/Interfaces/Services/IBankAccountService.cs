using System;
using System.Threading.Tasks;
using MBD.Transactions.Domain.ValueObjects;

namespace MBD.Transactions.Domain.Interfaces.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetByIdAsync(Guid id);
    }
}
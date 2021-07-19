using System;
using System.Threading.Tasks;
using MBD.CreditCards.Domain.ValueObjects;

namespace MBD.CreditCards.Domain.Interfaces.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetByIdAsync(Guid id);
    }
}
using MBD.BankAccounts.Domain.Entities;
using MBD.Core.Data;

namespace MBD.BankAccounts.Domain.Interfaces.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
    }
}
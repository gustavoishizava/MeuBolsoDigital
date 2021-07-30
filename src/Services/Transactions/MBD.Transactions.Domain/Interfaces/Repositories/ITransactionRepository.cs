using MBD.Core.Data;
using MBD.Transactions.Domain.Entities;

namespace MBD.Transactions.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
    }
}
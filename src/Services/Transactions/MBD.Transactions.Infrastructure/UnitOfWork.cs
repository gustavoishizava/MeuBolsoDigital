using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.Transactions.Infrastructure.Context;

namespace MBD.Transactions.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransactionContext _context;

        public UnitOfWork(TransactionContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
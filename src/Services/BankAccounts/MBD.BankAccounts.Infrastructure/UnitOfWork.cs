using System.Threading.Tasks;
using MBD.BankAccounts.Infrastructure.Context;
using MBD.Core.Data;

namespace MBD.BankAccounts.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankAccountContext _context;

        public UnitOfWork(BankAccountContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
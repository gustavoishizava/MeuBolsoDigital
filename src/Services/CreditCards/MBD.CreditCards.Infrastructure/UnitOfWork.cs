using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.CreditCards.Infrastructure.Context;

namespace MBD.CreditCards.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CreditCardContext _context;

        public UnitOfWork(CreditCardContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
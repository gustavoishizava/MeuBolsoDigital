using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.Identity.Infrastructure.Context;

namespace MBD.Identity.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityContext _context;

        public UnitOfWork(IdentityContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
using System;
using System.Threading.Tasks;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MBD.Transactions.Infrastructure.Context;

namespace MBD.Transactions.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext _context;

        public TransactionRepository(TransactionContext context)
        {
            _context = context;
        }

        public void Add(Transaction transaction)
        {
            _context.Add(transaction);
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public void Remove(Transaction transaction)
        {
            _context.Remove(transaction);
        }

        public void Update(Transaction transaction)
        {
            _context.Update(transaction);
        }
    }
}
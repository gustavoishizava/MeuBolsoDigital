using System;
using System.Threading.Tasks;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MBD.Transactions.Infrastructure.Context;

namespace MBD.Transactions.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly TransactionContext _context;

        public BankAccountRepository(TransactionContext context)
        {
            _context = context;
        }

        public void Add(BankAccount bankAccount)
        {
            _context.Add(bankAccount);
        }

        public void Update(BankAccount bankAccount)
        {
            _context.Update(bankAccount);
        }

        public async Task<BankAccount> GetByIdAsync(Guid id)
        {
            return await _context.BankAccounts.FindAsync(id);
        }
    }
}
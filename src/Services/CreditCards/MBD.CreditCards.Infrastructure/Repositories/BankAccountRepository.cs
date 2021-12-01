using System;
using System.Threading.Tasks;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MBD.CreditCards.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MBD.CreditCards.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly CreditCardContext _context;

        public BankAccountRepository(CreditCardContext context)
        {
            _context = context;
        }

        public void Add(BankAccount bankAccount)
        {
            _context.Add(bankAccount);
        }

        public async Task<BankAccount> GetByIdAsync(Guid id)
        {
            return await _context.BankAccounts.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
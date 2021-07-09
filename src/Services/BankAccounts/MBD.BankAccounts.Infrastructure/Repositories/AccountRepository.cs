using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBD.BankAccounts.Domain.Entities;
using MBD.BankAccounts.Domain.Interfaces.Repositories;
using MBD.BankAccounts.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MBD.BankAccounts.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankAccountContext _context;
        
        public AccountRepository(BankAccountContext context)
        {
            _context = context;
        }

        public void Add(Account account)
        {
            _context.Add(account);
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public void Remove(Account account)
        {
            _context.Remove(account);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(Account account)
        {
            _context.Update(account);
        }
    }
}
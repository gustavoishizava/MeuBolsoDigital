using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MBD.CreditCards.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MBD.CreditCards.Infrastructure.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly CreditCardContext _context;

        public CreditCardRepository(CreditCardContext context)
        {
            _context = context;
        }

        public void Add(CreditCard entity)
        {
            _context.Add(entity);
        }

        public async Task<CreditCard> GetByIdAsync(Guid id)
        {
            return await _context.CreditCards.FindAsync(id);
        }

        public void Remove(CreditCard entity)
        {
            _context.Remove(entity);
        }

        public void Update(CreditCard entity)
        {
            _context.Update(entity);
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync()
        {
            return await _context.CreditCards.ToListAsync();
        }
    }
}
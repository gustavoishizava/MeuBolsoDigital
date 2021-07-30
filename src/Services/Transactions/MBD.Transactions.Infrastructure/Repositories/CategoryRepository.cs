using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MBD.Transactions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MBD.Transactions.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TransactionContext _context;

        public CategoryRepository(TransactionContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Add(category);
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetByTypeAsync(TransactionType type)
        {
            return await _context.Categories
                    .AsNoTracking()
                    .Where(x => x.Type == type)
                    .ToListAsync();
        }

        public void Remove(Category category)
        {
            _context.Remove(category);
        }

        public void Update(Category category)
        {
            _context.Update(category);
        }
    }
}
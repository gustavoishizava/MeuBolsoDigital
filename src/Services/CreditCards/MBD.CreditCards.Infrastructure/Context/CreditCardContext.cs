using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MBD.CreditCards.Domain.Entities;
using MBD.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MBD.CreditCards.Infrastructure.Context
{
    public class CreditCardContext : DbContext
    {
        public CreditCardContext(DbContextOptions<CreditCardContext> options) : base(options)
        {
        }

        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<CreditCardBill> CreditCardBills { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateDateBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
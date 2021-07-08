using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Domain.Entities;
using MBD.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MBD.BankAccounts.Infrastructure.Context
{
    public class BankAccountContext : DbContext
    {
        public BankAccountContext(DbContextOptions<BankAccountContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
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
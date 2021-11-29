using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Identity;
using MBD.CreditCards.Domain.Entities;
using MBD.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MBD.CreditCards.Infrastructure.Context
{
    public class CreditCardContext : DbContext
    {
        private readonly IAspNetUser _aspNetUser;

        public CreditCardContext(DbContextOptions<CreditCardContext> options, IAspNetUser aspNetUser) : base(options)
        {
            _aspNetUser = aspNetUser;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<CreditCardBill> CreditCardBills { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<CreditCard>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
            modelBuilder.Entity<BankAccount>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateDateBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
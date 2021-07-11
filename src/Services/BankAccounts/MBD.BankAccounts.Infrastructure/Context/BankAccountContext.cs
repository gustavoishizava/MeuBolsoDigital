using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Domain.Entities;
using MBD.Core.Identity;
using MBD.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MBD.BankAccounts.Infrastructure.Context
{
    public class BankAccountContext : DbContext
    {
        private readonly IAspNetUser _aspNetUser;

        public BankAccountContext(DbContextOptions<BankAccountContext> options, IAspNetUser aspNetUser) : base(options)
        {
            _aspNetUser = aspNetUser;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Account>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateDateBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
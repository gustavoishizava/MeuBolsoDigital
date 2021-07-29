using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Identity;
using MBD.Infrastructure.Core.Extensions;
using MBD.Transactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MBD.Transactions.Infrastructure.Context
{
    public class TransactionContext : DbContext
    {
        private readonly IAspNetUser _aspNetUser;
        public TransactionContext(DbContextOptions<TransactionContext> options, IAspNetUser aspNetUser) : base(options)
        {
            _aspNetUser = aspNetUser;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Category>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
            modelBuilder.Entity<Transaction>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateDateBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
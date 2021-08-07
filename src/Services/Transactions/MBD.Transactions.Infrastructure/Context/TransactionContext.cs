using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Identity;
using MBD.Infrastructure.Core.Extensions;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Events.Common;
using MBD.Transactions.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MBD.Transactions.Infrastructure.Context
{
    public class TransactionContext : DbContext
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly IMediator _mediator;

        public TransactionContext(DbContextOptions<TransactionContext> options, IAspNetUser aspNetUser, IMediator mediator) : base(options)
        {
            _aspNetUser = aspNetUser;
            _mediator = mediator;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<DomainEvent>();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Category>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
            modelBuilder.Entity<Transaction>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateDateBeforeSaving();

            var transactionResult = await base.SaveChangesAsync(cancellationToken);

            if (transactionResult > 0)
                await _mediator.DispatchDomainEventsAsync(this);

            return transactionResult;
        }
    }
}
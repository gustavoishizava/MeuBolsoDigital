using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MBD.BankAccounts.Domain.Entities;
using MBD.BankAccounts.Domain.Events.Common;
using MBD.BankAccounts.Infrastructure.Extensions;
using MBD.Core.Identity;
using MBD.Infrastructure.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MBD.BankAccounts.Infrastructure.Context
{
    public class BankAccountContext : DbContext
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly IMediator _mediator;

        public BankAccountContext(DbContextOptions<BankAccountContext> options, IAspNetUser aspNetUser, IMediator mediator) : base(options)
        {
            _aspNetUser = aspNetUser;
            _mediator = mediator;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<DomainEvent>();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Account>().HasQueryFilter(x => x.TenantId == _aspNetUser.UserId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateDateBeforeSaving();
            var result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
                await _mediator.DispatchDomainEventsAsync(this);

            return result;
        }
    }
}
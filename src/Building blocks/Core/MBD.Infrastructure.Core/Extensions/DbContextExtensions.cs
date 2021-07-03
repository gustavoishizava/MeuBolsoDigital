using System;
using System.Linq;
using MBD.Core.Entities;
using MBD.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MBD.Infrastructure.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static void UpdateDateBeforeSaving(this DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries<BaseEntity>()
                            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            if (entries.IsNullOrEmpty())
                return;

            foreach (var entry in entries)
            {
                var entryUpdatedAt = entry.Property(nameof(BaseEntity.UpdatedAt));

                if (entry.State == EntityState.Added)
                {
                    entryUpdatedAt.CurrentValue = null;
                    entryUpdatedAt.IsModified = false;
                }
                else
                {
                    entryUpdatedAt.CurrentValue = DateTime.Now;
                }
            }
        }
    }
}
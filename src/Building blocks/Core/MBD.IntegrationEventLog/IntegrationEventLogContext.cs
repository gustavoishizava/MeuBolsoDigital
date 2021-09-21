using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBD.IntegrationEventLog
{
    public class IntegrationEventLogContext : DbContext
    {
        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
        {
        }

        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLog);
        }

        private void ConfigureIntegrationEventLog(EntityTypeBuilder<IntegrationEventLogEntry> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.EventTypeName)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");

            builder.Property(x => x.Content)
                .IsRequired();
        }
    }
}
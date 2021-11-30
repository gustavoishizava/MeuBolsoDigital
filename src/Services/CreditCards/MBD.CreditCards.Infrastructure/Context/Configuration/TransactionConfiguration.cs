using MBD.CreditCards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBD.CreditCards.Infrastructure.Context.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(x => x.CreditCardBillId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.Value)
                .HasColumnType("NUMERIC(18,2)")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne<CreditCardBill>()
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.CreditCardBillId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
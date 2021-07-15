using MBD.CreditCards.Domain.Entities;
using MBD.Infrastructure.Core.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBD.CreditCards.Infrastructure.Context.Configuration
{
    public class CreditCardBillConfiguration : BaseEntityConfiguration<CreditCardBill>
    {
        public override void Configure(EntityTypeBuilder<CreditCardBill> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.CreditCardId)
                .IsRequired();

            builder.Property(x => x.ClosesIn)
                .IsRequired();

            builder.Property(x => x.DueDate)
                .IsRequired();

            builder.OwnsOne(x => x.Reference, reference =>
            {
                reference.Property(x => x.Month)
                    .IsRequired();

                reference.Property(x => x.Year)
                    .IsRequired();
            });

            builder.HasOne<CreditCard>()
                .WithMany()
                .HasForeignKey(x => x.CreditCardId);

            builder.HasIndex(x => 
                new { x.CreditCardId, x.Reference.Month, x.Reference.Year });
        }
    }
}
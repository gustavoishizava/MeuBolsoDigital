using MBD.Core.Enumerations;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Enumerations;
using MBD.Infrastructure.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MBD.CreditCards.Infrastructure.Context.Configuration
{
    public class CreditCardConfiguration : BaseEntityConfiguration<CreditCard>
    {
        public override void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.TenantId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100);

            builder.Property(x => x.ClosingDay)
                .IsRequired();

            builder.Property(x => x.DayOfPayment)
                .IsRequired();

            builder.Property(x => x.Limit)
                .IsRequired()
                .HasColumnType("NUMERIC(18,2)")
                .HasPrecision(18, 2);

            builder.Property(x => x.Brand)
                .IsRequired()
                .HasColumnType("VARCHAR(20)")
                .HasMaxLength(20)
                .HasConversion(new EnumToStringConverter<Brand>());

            builder.Property(x => x.Status)
                .IsRequired()
                .HasColumnType("VARCHAR(10)")
                .HasMaxLength(10)
                .HasConversion(new EnumToStringConverter<Status>());

            builder.HasOne(x => x.BankAccount)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Bills)
                .HasField("_bills");
        }
    }
}
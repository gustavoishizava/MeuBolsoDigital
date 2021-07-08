using MBD.BankAccounts.Domain.Entities;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Infrastructure.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MBD.BankAccounts.Infrastructure.Context.Configuration
{
    public class BankAccountConfiguration : BaseEntityConfiguration<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasColumnType("VARCHAR(150)")
                .HasMaxLength(150);

            builder.Property(x => x.InitialBalance)
                .IsRequired()
                .HasColumnType("NUMERIC(18,2)")
                .HasPrecision(18, 2);
            
            builder.Property(x => x.Type)
                .IsRequired()
                .HasColumnType("VARCHAR(20)")
                .HasMaxLength(20)
                .HasConversion(new EnumToStringConverter<AccountType>());
            
            builder.Property(x => x.Status)
                .IsRequired()
                .HasColumnType("VARCHAR(10)")
                .HasMaxLength(10)
                .HasConversion(new EnumToStringConverter<AccountType>());
        }
    }
}
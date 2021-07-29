using MBD.Core.Enumerations;
using MBD.Infrastructure.Core.Configuration;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MBD.Transactions.Infrastructure.Context.Configuration
{
    public class CategoryConfiguration : BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.TenantId)
                .IsRequired();

            builder.Property(x => x.ParentCategoryId)
                .IsRequired(false);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(250)")
                .HasMaxLength(250);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasColumnType("VARCHAR(10)")
                .HasMaxLength(10)
                .HasConversion(new EnumToStringConverter<TransactionType>());

            builder.Property(x => x.Status)
                .IsRequired()
                .HasColumnType("VARCHAR(10)")
                .HasMaxLength(10)
                .HasConversion(new EnumToStringConverter<Status>());

            builder.HasMany(x => x.SubCategories)
                .WithOne()
                .HasForeignKey(x => x.ParentCategoryId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Navigation(x => x.SubCategories)
                .HasField("_subCategories");
        }
    }
}
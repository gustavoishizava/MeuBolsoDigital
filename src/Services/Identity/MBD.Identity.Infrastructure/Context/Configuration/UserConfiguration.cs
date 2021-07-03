using MBD.Identity.Domain.Entities;
using MBD.Infrastructure.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBD.Identity.Infrastructure.Context.Configuration
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            
            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("VARCHAR(150)")
                .HasMaxLength(100);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasColumnName("VARCHAR(250)")
                .HasMaxLength(250);

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
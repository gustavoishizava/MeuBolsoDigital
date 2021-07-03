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

            builder.OwnsOne(x => x.Email, email =>
            {
                email.Property(e => e.Address)
                    .IsRequired(true)
                    .HasColumnName("Email")
                    .HasColumnType("VARCHAR(150)")
                    .HasMaxLength(150);

                email.Property(e => e.NormalizedAddress)
                    .IsRequired(true)
                    .HasColumnName("NormalizedEmail")
                    .HasColumnType("VARCHAR(150)")
                    .HasMaxLength(150);

                email.HasIndex(e => e.Address).IsUnique();
            });

            builder.OwnsOne(x => x.Password, password =>
            {
                password.Property(p => p.PasswordHash)
                                .IsRequired()
                                .HasColumnName("VARCHAR(250)")
                                .HasMaxLength(250);
            });
        }
    }
}
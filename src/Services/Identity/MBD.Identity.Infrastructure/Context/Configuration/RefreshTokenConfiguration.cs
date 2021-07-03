using MBD.Identity.Domain.Entities;
using MBD.Infrastructure.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBD.Identity.Infrastructure.Context.Configuration
{
    public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
    {        
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);
            
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Token)
                .IsRequired()
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100);

            builder.Property(x => x.ExpiresIn)
                .IsRequired();

            builder.Property(x => x.RevokedOn)
                .IsRequired(false);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
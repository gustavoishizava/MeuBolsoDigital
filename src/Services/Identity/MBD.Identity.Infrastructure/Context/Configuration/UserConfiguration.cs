using MBD.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBD.Identity.Infrastructure.Context.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

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
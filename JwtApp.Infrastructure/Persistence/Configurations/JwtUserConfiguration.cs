using Jwt.Domain;
using JwtApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JwtApp.Infrastructure.Persistence.Configurations;

public class JwtUserConfiguration : IEntityTypeConfiguration<JwtUser>
{
    public void Configure(EntityTypeBuilder<JwtUser> builder)
    {
        builder.ToTable($"{nameof(JwtUser)}s");
        builder.HasKey(x => x.JwtUserId);

        builder.Property(a => a.Name)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(u => u.CreatedTime)
            .HasDefaultValueSql("GETDATE()")
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

        builder.HasOne<ApplicationUser>()
            .WithOne(f => f.JwtUser)
            .HasForeignKey<JwtUser>(u => u.JwtUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
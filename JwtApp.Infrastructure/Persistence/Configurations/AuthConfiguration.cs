using JwtApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JwtApp.Infrastructure.Persistence.Configurations;

public static class AuthConfiguration
{
    private const string Schema = "auth";

    public static void ApplyIdentityConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(b =>
        {
            b.ToAuthTable($"{nameof(ApplicationUser)}s");
            b.HasKey(u => u.Id);

            b.HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<IdentityUserClaim<string>>(b => { b.ToAuthTable("IdUserClaims"); });
        builder.Entity<IdentityUserLogin<string>>(b => { b.ToAuthTable("IdUserLogins"); });
        builder.Entity<IdentityUserToken<string>>(b => { b.ToAuthTable("IdUserTokens"); });
    }

    private static EntityTypeBuilder<TEntity> ToAuthTable<TEntity>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder,
        string name)
        where TEntity : class
        => entityTypeBuilder.ToTable(name, Schema);
}
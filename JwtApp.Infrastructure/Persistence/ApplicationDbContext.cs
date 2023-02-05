using Jwt.Domain;
using JwtApp.Application.Interfaces;
using JwtApp.Infrastructure.Models;
using JwtApp.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtApp.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityUserContext<ApplicationUser>, IJwtDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<JwtUser> JwtUsers => Set<JwtUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyIdentityConfiguration();
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
using JwtApp.Application.Interfaces;
using JwtApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JwtApp.Infrastructure;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration conf)
    {
        var connectionString = conf.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IJwtDbContext>(p => p.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
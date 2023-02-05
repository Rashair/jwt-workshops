using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JwtApp.Application;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ConfigurationExtensions).Assembly);

        return services;
    }
}
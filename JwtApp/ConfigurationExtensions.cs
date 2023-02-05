using Microsoft.OpenApi.Models;

namespace JwtApp;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration conf)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new() { Title = "JWT API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
namespace JwtApp;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration conf)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
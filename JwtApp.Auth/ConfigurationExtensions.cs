using System.Text;
using JwtApp.Data;
using JwtApp.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JwtApp.Auth;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration conf)
    {
        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => o.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = conf["Jwt:Audience"],
                ValidIssuer = conf["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(conf["Jwt:Key"]!)),
                ClockSkew = TimeSpan.FromSeconds(10),
            });

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
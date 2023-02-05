using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtApp.Infrastructure.Auth.Behaviours;
using JwtApp.Infrastructure.Auth.Models;
using JwtApp.Infrastructure.Models;
using JwtApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JwtApp.Infrastructure.Auth;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration conf)
    {
        // Clear default .Net claim maps
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.SignIn.RequireConfirmedAccount = false;
                o.Password.RequiredLength = 16;
                o.Password.RequireDigit = false;
                o.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => o.TokenValidationParameters = new()
            {
                RoleClaimType = Claims.Role,
                ValidAudience = conf["Jwt:Audience"],
                ValidIssuer = conf["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(conf["Jwt:Key"]!)),
                ClockSkew = TimeSpan.FromSeconds(10),
            });

        services.AddAuthorization(o =>
        {
            o.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            o.AddPolicy(Policies.AdminOnly, p => p.RequireRole(Roles.Admin));
            o.AddPolicy(Policies.IsEmployee, p => p.RequireClaim(Claims.Employee));
            o.AddPolicy(Policies.IsAdult, p => p.RequireAssertion(a =>
                a.User.HasClaim(c => c.Type == Claims.Age
                                     && int.TryParse(c.Value, out var ageVal)
                                     && ageVal >= 18
                )));
        });

        services.AddHttpContextAccessor();
        services.AddScoped<SignInManager<ApplicationUser>>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUsersService, UsersService>();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserIdBehaviour<,>));

        return services;
    }
}
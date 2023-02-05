using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JwtApp.Infrastructure.Auth.Models;
using JwtApp.Infrastructure.Models;
using JwtApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtApp.Infrastructure.Auth;

public interface IAuthService
{
    Task<string> Login(LoginCommand loginCommand);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _ctx;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(IConfiguration configuration,
        ApplicationDbContext ctx,
        SignInManager<ApplicationUser> signInManager)
    {
        _configuration = configuration;
        _ctx = ctx;
        _signInManager = signInManager;
    }

    public async Task<string> Login(LoginCommand command)
    {
        var normalizedUsername = _signInManager.UserManager.NormalizeName(command.Username);
        var user = await _ctx.Users
            .Where(u => u.NormalizedUserName == normalizedUsername)
            .Include(u => u.Claims)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            return null;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, true);
        if (!signInResult.Succeeded)
        {
            return null;
        }

        var claims = CreateUserClaims(user);

        return CreateAccessToken(claims);
    }

    private static Dictionary<string, object> CreateUserClaims(ApplicationUser user)
    {
        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            [JwtRegisteredClaimNames.Iat] = DateTime.UtcNow.ToString(),
            [JwtRegisteredClaimNames.NameId] = user.Id,
            [JwtRegisteredClaimNames.Name] = user.UserName,
            [Claims.Age] = user.Age,
            [JwtRegisteredClaimNames.Email] = user.Email,
        };

        foreach (var userClaim in user.Claims)
        {
            claims.Add(userClaim.ClaimType, userClaim.ClaimValue);
        }

        return claims;
    }


    private string CreateAccessToken(Dictionary<string, object> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new()
        {
            Audience = _configuration["Jwt:Audience"],
            Issuer = _configuration["Jwt:Issuer"],
            Claims = claims,
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes")),
            SigningCredentials = signingCredentials,
        });

        return tokenHandler.WriteToken(token);
    }
}
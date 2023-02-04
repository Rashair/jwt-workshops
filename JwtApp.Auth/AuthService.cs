using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JwtApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtApp.Auth;

public interface IAuthService
{
    Task<ApplicationUser> SignIn(string username, string password);
    Task<string> CreateAccessToken(ApplicationUser user);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ApplicationUser> SignIn(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return null;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!signInResult.Succeeded)
        {
            return null;
        }

        return user;
    }

    public async Task<string> CreateAccessToken(ApplicationUser user)
    {
        //create claims details based on the user information
        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            [JwtRegisteredClaimNames.Iat] = DateTime.UtcNow.ToString(),
            [JwtRegisteredClaimNames.NameId] = user.Id,
            [JwtRegisteredClaimNames.Name] = user.UserName,
            ["age"] = user.Age,
            ["email"] = user.Email
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new()
        {
            Audience = _configuration["Jwt:Audience"],
            Issuer = _configuration["Jwt:Issuer"],
            Claims = claims,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = signIn,
        });

        return tokenHandler.WriteToken(token);
    }
}
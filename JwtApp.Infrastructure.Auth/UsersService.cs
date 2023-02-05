using Jwt.Domain;
using JwtApp.Infrastructure.Auth.Models;
using JwtApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace JwtApp.Infrastructure.Auth;

public interface IUsersService
{
    Task<IdentityResult> CreateUser(CreateUserCommand command);
}

public class UsersService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUser(CreateUserCommand command)
    {
        return await _userManager.CreateAsync(new()
        {
            UserName = command.Username,
            Age = command.Age,
            Email = command.Email,
            JwtUser = new()
            {
                Name = command.Name,
            },
            Claims = command.Claims.Select(c => new IdentityUserClaim<string>()
            {
                ClaimType = c.Type,
                ClaimValue = c.Value,
            }).ToList()
        }, command.Password);
    }
}
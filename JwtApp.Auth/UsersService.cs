using JwtApp.Auth.Models;
using JwtApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace JwtApp.Auth;

public interface IUsersService
{
    Task CreateUser(CreateUserCommand command);
}

public class UsersService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task CreateUser(CreateUserCommand command)
    {
        await _userManager.CreateAsync(new()
        {
            UserName = command.Username,
            Age = command.Age,
            Email = command.Email,
        },command.Password);
    }
}
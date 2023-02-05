using JwtApp.Infrastructure.Auth;
using JwtApp.Infrastructure.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUsersService _usersService;

    public AuthController(IAuthService authService, IUsersService usersService)
    {
        _authService = authService;
        _usersService = usersService;
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login(LoginCommand command)
    {
        var token = await _authService.Login(command);
        if (token == null)
        {
            return BadRequest("Invalid credentials");
        }

        return Ok(token);
    }

    [HttpPost("create-user")]
    [Authorize(Policies.AdminOnly)]
    public async Task<ActionResult<string>> CreateUser(CreateUserCommand command)
    {
        var res = await _usersService.CreateUser(command);
        if (!res.Succeeded)
        {
            return BadRequest(new { errors = res.Errors });
        }

        return Ok();
    }
}
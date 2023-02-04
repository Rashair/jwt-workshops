namespace JwtApp.Controllers.Models;

public class LoginCommand
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
namespace JwtApp.Auth.Models;

public class CreateUserCommand
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required int Age { get; set; }
    public string Email { get; set; }
}
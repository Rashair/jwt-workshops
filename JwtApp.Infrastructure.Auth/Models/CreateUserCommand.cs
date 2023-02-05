using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace JwtApp.Infrastructure.Auth.Models;

public class CreateUserCommand
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required int Age { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

    public List<ClaimDto> Claims { get; set; }
}

public class ClaimDto
{
    public ClaimDto()
    {
    }

    [SetsRequiredMembers]
    public ClaimDto(string type, string value = null)
    {
        Type = type;
        Value = value;
    }


    public required string Type { get; set; }
    public required string Value { get; set; }
}
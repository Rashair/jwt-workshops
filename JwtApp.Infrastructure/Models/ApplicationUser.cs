using Jwt.Domain;
using Microsoft.AspNetCore.Identity;

namespace JwtApp.Infrastructure.Models;

public class ApplicationUser : IdentityUser
{
    public int Age { get; set; }

    public IList<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
    public required JwtUser JwtUser { get; init; } = null!;
}
using Microsoft.AspNetCore.Identity;

namespace JwtApp.Infrastructure.Models;

public class ApplicationUser : IdentityUser
{
    public IList<IdentityUserClaim<string>> Claims { get; set; }

    public int Age { get; set; }
}



using Microsoft.AspNetCore.Identity;

namespace JwtApp.Infrastructure.Models;

public class ApplicationUser : IdentityUser
{
    public int Age { get; set; }
}
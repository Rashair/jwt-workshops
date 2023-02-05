using JwtApp.Auth;
using JwtApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JwtApp.Utils;

public static class DevExtensions
{
    public static async Task UseDevExtensions(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await db.Users.AnyAsync(u => u.NormalizedUserName == "ADMIN"))
        {
            return;
        }

        var service = scope.ServiceProvider.GetRequiredService<IUsersService>();
        await service.CreateUser(new()
        {
            Username = "admin",
            Age = 30,
            Password = "happy-admin-2023",
            Email = "admin@admin.com"
        });
    }
}
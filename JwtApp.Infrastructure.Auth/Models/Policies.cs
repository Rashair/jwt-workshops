namespace JwtApp.Infrastructure.Auth.Models;

public static class Policies
{
    public const string AdminOnly = nameof(AdminOnly);
    public const string IsEmployee = nameof(IsEmployee);
    public const string IsAdult = nameof(IsAdult);
}

public static class Roles
{
    public const string Admin = "admin";
}

public static class Claims
{
    public const string Role = "role";
    public const string Employee = "employee";
    public const string Age = "age";
}

namespace Jwt.Domain;

public class JwtUser
{
    public string JwtUserId { get; set; }
    public string Name { get; set; }

    public DateTimeOffset CreatedTime { get; set; }
}
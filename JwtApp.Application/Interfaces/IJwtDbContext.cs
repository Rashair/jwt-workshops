using Jwt.Domain;
using Microsoft.EntityFrameworkCore;

namespace JwtApp.Application.Interfaces;

public interface IJwtDbContext
{
    DbSet<JwtUser> JwtUsers { get; }
}
using JwtApp.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JwtApp.Application;

public class GetUsersCountQuery : IRequest<int>
{
}

public class GetUsersCountHandler : IRequestHandler<GetUsersCountQuery, int>
{
    private readonly IJwtDbContext _ctx;

    public GetUsersCountHandler(IJwtDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> Handle(GetUsersCountQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.JwtUsers.CountAsync(cancellationToken: cancellationToken);
    }
}
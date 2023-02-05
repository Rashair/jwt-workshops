using JwtApp.Application.Common;
using JwtApp.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JwtApp.Application;

public class GetUserDataQuery : UserRequest<GetUserDataResult>
{
}

public class GetUserDataHandler : IRequestHandler<GetUserDataQuery, GetUserDataResult>
{
    private readonly IJwtDbContext _ctx;

    public GetUserDataHandler(IJwtDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GetUserDataResult> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.JwtUsers
            .Where(u => u.JwtUserId == request.UserId)
            .Select(u => new GetUserDataResult()
            {
                Name = u.Name,
                CreatedTime = u.CreatedTime
            })
            .SingleAsync(cancellationToken);
    }
}

public class GetUserDataResult
{
    public required string Name { get; init; }
    public required DateTimeOffset CreatedTime { get; init; }
};
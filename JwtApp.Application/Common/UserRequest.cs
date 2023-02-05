using MediatR;

namespace JwtApp.Application.Common;

public class UserRequest : IRequest
{
    public string UserId { get; set; } = null!;
}

public class UserRequest<TResult> : IRequest<TResult>
{
    public string UserId { get; set; } = null!;
}
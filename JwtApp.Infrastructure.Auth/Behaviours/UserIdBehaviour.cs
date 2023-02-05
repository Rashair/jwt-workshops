using System.IdentityModel.Tokens.Jwt;
using JwtApp.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace JwtApp.Infrastructure.Auth.Behaviours;

public class UserIdBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpAuthAccessor;

    public UserIdBehaviour(IHttpContextAccessor httpAuthAccessor)
    {
        _httpAuthAccessor = httpAuthAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        switch (request)
        {
            case UserRequest<TResponse> userRequest:
                userRequest.UserId = GetUserId();
                break;
            case UserRequest userRequestWithoutResponse:
                userRequestWithoutResponse.UserId = GetUserId();
                break;
        }

        return await next();
    }

    private string GetUserId()
    {
        if (_httpAuthAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("No http context");
        }

        return _httpAuthAccessor.HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.NameId).Value;
    }
}
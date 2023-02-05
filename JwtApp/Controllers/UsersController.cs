using JwtApp.Application;
using JwtApp.Infrastructure.Auth.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("data")]
    public async Task<ActionResult<GetUserDataResult>> GetData()
    {
        return await _mediator.Send(new GetUserDataQuery());
    }


    [HttpGet("count")]
    [Authorize(Policies.IsEmployee)]
    public async Task<ActionResult<int>> GetCount()
    {
        return await _mediator.Send(new GetUsersCountQuery());
    }


}
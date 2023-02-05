﻿using System.Net;
using JwtApp.Auth;
using JwtApp.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration config, IAuthService authService)
    {
        _configuration = config;
        _authService = authService;
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login(LoginCommand command)
    {
        var user = await _authService.SignIn(command.Username, command.Password);
        if (user == null)
        {
            return BadRequest("Invalid credentials");
        }

        var token = await _authService.CreateAccessToken(user);
        return Ok(token);
    }
}
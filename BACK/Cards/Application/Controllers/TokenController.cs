namespace Cards.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using Cards.Domain.Interfaces;
using Cards.Application.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("login")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public TokenController(ITokenService tokenService, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _configuration = configuration;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Post(Request user)
    {
        if (user.Login != _configuration["User:Login"] || user.Password != _configuration["User:Password"])
        {
            return Unauthorized();
        }

        var token = _tokenService.BuildToken(_configuration["Jwt:Key"], _configuration["Jwt:Issuer"], user.Login);
        return Ok(new Response(token));
    }
}

using JwtApp.Controllers.Models;
using JwtApp.Infrastructure.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policies.IsAdult)]
public class SensitiveContentController : ControllerBase
{
    private static readonly string[] Names =
    {
        "Rothmans", "Winfield", "Benson & Hedges", "Dunhill", "Bond Street", "Chesterfield", "Marlboro",
    };

    private readonly ILogger<SensitiveContentController> _logger;

    public SensitiveContentController(ILogger<SensitiveContentController> logger)
    {
        _logger = logger;
    }

    [HttpGet("cigarettes")]
    public IEnumerable<Cigarette> GetCigarettesToBuy()
    {
        return Enumerable.Range(1, 5).Select(index => new Cigarette
            {
                Name = Names[Random.Shared.Next(Names.Length)],
                Price = (decimal) (20 + Random.Shared.NextDouble() * 20)
            })
            .ToArray();
    }
}
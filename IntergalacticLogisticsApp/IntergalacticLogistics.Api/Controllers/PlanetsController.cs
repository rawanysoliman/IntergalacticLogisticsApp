using IntergalacticLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanetsController : Controller
    {
        private readonly ISwapiClient _swapiClient;
        public PlanetsController(ISwapiClient swapiClient)
        {
            _swapiClient = swapiClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanet(string id)
        {
            var planet = await _swapiClient.GetPlanetByIdAsync(id);
            if (planet == null)
            {
                return NotFound($"Planet with id '{id}' not found.");
            }
            return Ok(planet);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlanets(CancellationToken cancellationToken)
        {
            var planets = await _swapiClient.GetPlanetsAsync(cancellationToken);
            if (planets == null || !planets.Any())
                return NotFound("No planets found.");
            return Ok(planets);
        }

    }
}

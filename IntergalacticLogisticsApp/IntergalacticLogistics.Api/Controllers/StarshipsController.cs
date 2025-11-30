using IntergalacticLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StarshipsController : Controller
    {
        private readonly ISwapiClient _swapiClient;

        public StarshipsController(ISwapiClient swapiClient)
        {
            _swapiClient = swapiClient;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStarships(CancellationToken cancellationToken)
        {
            var starships = await _swapiClient.GetStarshipsAsync(cancellationToken);
            if (starships == null || !starships.Any())
                return NotFound("No starships found.");

            var dtoList = starships.Select(s => new
            {
                s.Name,
                s.Model,
                s.Manufacturer,
                costInCredits = s.CostInCredits,
                s.Length,
                maxAtmospheringSpeed = s.MaxAtmospheringSpeed,
                s.Crew,
                s.Passengers,
                cargoCapacity = s.CargoCapacity,
                hyperdriveRating = s.HyperdriveRating,
                s.Mglt,
                starshipClass = s.StarshipClass,
                s.Url,
                s.SwapiId
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStarship(string id)
        {
          var starShip = await _swapiClient.GetStarshipByIdAsync(id);
          if (starShip == null)
          {
              return NotFound($"Starship with id '{id}' not found.");
            }
            var dtoList = new
            {
                starShip.Name,
                starShip.Model,
                starShip.Manufacturer,
                costInCredits = starShip.CostInCredits,
                starShip.Length,
                maxAtmospheringSpeed = starShip.MaxAtmospheringSpeed,
                starShip.Crew,
                starShip.Passengers,
                cargoCapacity = starShip.CargoCapacity,
                hyperdriveRating = starShip.HyperdriveRating,
                starShip.Mglt,
                starshipClass = starShip.StarshipClass,
                starShip.Url,
                starShip.SwapiId
            };
            return Ok(dtoList);
        }




    }
}

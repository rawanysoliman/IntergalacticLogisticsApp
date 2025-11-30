using IntergalacticLogistics.Application.Dtos;

namespace IntergalacticLogistics.Application.Interfaces;

public interface ISwapiClient
{
    Task<StarshipInfoDto?> GetStarshipByIdAsync(string starshipId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StarshipInfoDto>?> GetStarshipsAsync(CancellationToken cancellationToken = default);
    Task<PlanetInfoDto?> GetPlanetByIdAsync(string planetId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlanetInfoDto>?> GetPlanetsAsync(CancellationToken cancellationToken = default);
}


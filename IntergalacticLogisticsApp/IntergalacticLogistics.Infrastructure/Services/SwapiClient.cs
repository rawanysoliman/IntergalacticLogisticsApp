using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Application.Dtos;

namespace IntergalacticLogistics.Infrastructure.Services;

public class SwapiClient : ISwapiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SwapiClient> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private const string BaseUrl = "https://swapi.dev/api/";

    public SwapiClient(HttpClient httpClient, ILogger<SwapiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<StarshipInfoDto?> GetStarshipByIdAsync(string starshipId, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        _logger.LogInformation("Fetching starship from SWAPI: StarshipId={StarshipId}", starshipId);

        try
        {
            var response = await _httpClient.GetAsync($"starships/{starshipId}/", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch starship {StarshipId} from SWAPI. Status: {StatusCode}", starshipId, response.StatusCode);
                return null;
            }
            
            var starship = await response.Content.ReadFromJsonAsync<StarshipInfoDto>(JsonOptions, cancellationToken);
            if (starship != null)
            {
                starship.SwapiId = SwapiUrlParser.ExtractId(starship.Url);
                _logger.LogInformation(
                    "Starship fetched successfully: StarshipId={StarshipId}, Name={Name}, HyperdriveRating={Rating}",
                    starshipId, starship.Name, starship.HyperdriveRating);
            }
            else
            {
                _logger.LogWarning("Starship deserialization failed: StarshipId={StarshipId}",starshipId);
            }
            return starship;
        }
        catch (Exception ex)
        {
            var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
            _logger.LogError(ex,
                "Error fetching starship from SWAPI: StarshipId={StarshipId}",
                starshipId);
            return null;
        }
    }



    public async Task<PlanetInfoDto?> GetPlanetByIdAsync(string planetId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"planets/{planetId}/", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch planet {PlanetId} from SWAPI. Status: {StatusCode}", 
                    planetId, response.StatusCode);
                return null;
            }
            var planet = await response.Content.ReadFromJsonAsync<PlanetInfoDto>(JsonOptions, cancellationToken);
            return planet;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching planet {PlanetId} from SWAPI", planetId);
            return null;
        }
    }



    public async Task<IEnumerable<StarshipInfoDto>?> GetStarshipsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("starships/", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch starships from SWAPI. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            var swapiResponse = await response.Content.ReadFromJsonAsync<SwapiResponseDto<StarshipInfoDto>>(JsonOptions, cancellationToken);
            if (swapiResponse?.Results != null)
            {
                foreach (var starship in swapiResponse.Results)
                {
                    starship.SwapiId = SwapiUrlParser.ExtractId(starship.Url);
                }
            }
            return swapiResponse?.Results ?? Enumerable.Empty<StarshipInfoDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching starships from SWAPI");
            return null;
        }
    }




    public async Task<IEnumerable<PlanetInfoDto>?> GetPlanetsAsync(CancellationToken cancellationToken = default)
    {
        try 
        {
            var response = await _httpClient.GetAsync("planets/", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch planets from SWAPI. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            var swapiResponse = await response.Content.ReadFromJsonAsync<SwapiResponseDto<PlanetInfoDto>>(JsonOptions, cancellationToken);

            return swapiResponse?.Results ?? Enumerable.Empty<PlanetInfoDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching planets from SWAPI");
            return null;
        }
    }


}







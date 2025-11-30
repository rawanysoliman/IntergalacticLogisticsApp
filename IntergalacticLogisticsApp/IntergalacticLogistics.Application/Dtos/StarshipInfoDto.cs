using System.Linq;
using System.Text.Json.Serialization;

namespace IntergalacticLogistics.Application.Dtos;

public class StarshipInfoDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    [JsonPropertyName("cost_in_credits")]
    public string CostInCredits { get; set; } = string.Empty;

    [JsonPropertyName("length")]
    public string Length { get; set; } = string.Empty;

    [JsonPropertyName("max_atmosphering_speed")]
    public string MaxAtmospheringSpeed { get; set; } = string.Empty;

    [JsonPropertyName("crew")]
    public string Crew { get; set; } = string.Empty;

    [JsonPropertyName("passengers")]
    public string Passengers { get; set; } = string.Empty;

    [JsonPropertyName("cargo_capacity")]
    public string CargoCapacity { get; set; } = string.Empty;

    [JsonPropertyName("hyperdrive_rating")]
    public string HyperdriveRating { get; set; } = string.Empty;

    [JsonPropertyName("MGLT")]
    public string Mglt { get; set; } = string.Empty;

    [JsonPropertyName("starship_class")]
    public string StarshipClass { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("swapiId")]
    public string SwapiId { get; set; } = string.Empty; //extracted from swapi url
}

namespace IntergalacticLogistics.Application.Dtos;

//wrapper object from SWAPI responses
public class SwapiResponseDto<T>
{
    public int Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public List<T> Results { get; set; } = new();
}


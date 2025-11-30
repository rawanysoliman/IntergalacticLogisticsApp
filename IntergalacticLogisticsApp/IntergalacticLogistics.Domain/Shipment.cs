namespace IntergalacticLogistics.Domain;

public class Shipment
{
    public Guid Id { get; set; }
    public string StarshipId { get; set; } = string.Empty;
    public string StarshipName { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal CargoWeight { get; set; }
    public string ShippingMethod { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public DateTime CreatedAt { get; set; }
}


namespace IntergalacticLogistics.Application.Commands;

public record CreateShipmentCommand(
    string StarshipId,
    string StarshipName,
    string Origin,
    string Destination,
    decimal CargoWeight,
    string ShippingMethod
);


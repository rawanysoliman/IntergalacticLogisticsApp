using IntergalacticLogistics.Application.Commands;
using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Domain;
using Microsoft.Extensions.Logging;

namespace IntergalacticLogistics.Infrastructure.Services;

public class CreateShipmentProcessor : ICreateShipmentProcessor
{
    private readonly IShipmentRepository _repository;
    private readonly Func<string, IShippingCostStrategy> _strategyFactory;
    private readonly ILogger<CreateShipmentProcessor> _logger;

    public CreateShipmentProcessor(
        IShipmentRepository repository,
        Func<string, IShippingCostStrategy> strategyFactory,
        ILogger<CreateShipmentProcessor> logger)
    {
        _repository = repository;
        _strategyFactory = strategyFactory;
        _logger = logger;
    }

    public async Task<Shipment> HandleAsync(CreateShipmentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Processing shipment creation: StarshipId={StarshipId}, Origin={Origin}, Destination={Destination}, CargoWeight={CargoWeight}, ShippingMethod={ShippingMethod}",
            command.StarshipId, command.Origin, command.Destination, command.CargoWeight, command.ShippingMethod);

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            StarshipId = command.StarshipId,
            StarshipName = command.StarshipName,
            Origin = command.Origin,
            Destination = command.Destination,
            CargoWeight = command.CargoWeight,
            ShippingMethod = command.ShippingMethod,
            CreatedAt = DateTime.UtcNow
        };

        var strategy = _strategyFactory(command.ShippingMethod);
        shipment.Cost = await strategy.CalculateCostAsync(shipment, cancellationToken);

        await _repository.CreateAsync(shipment, cancellationToken);


        _logger.LogInformation(
            "Shipment created successfully: ShipmentId={ShipmentId}",
            shipment.Id);
        
        return shipment;
    }
}


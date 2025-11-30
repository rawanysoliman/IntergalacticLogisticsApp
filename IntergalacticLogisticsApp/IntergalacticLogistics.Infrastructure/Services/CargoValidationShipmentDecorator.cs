using IntergalacticLogistics.Application.Commands;
using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Domain;
using Microsoft.Extensions.Logging;

namespace IntergalacticLogistics.Infrastructure.Services;

public class CargoValidationShipmentDecorator : ICreateShipmentProcessor
{
    private readonly ICreateShipmentProcessor _inner;
    private readonly ILogger<CargoValidationShipmentDecorator> _logger;

    private const decimal MaxCargoWeight = 10000m;

    public CargoValidationShipmentDecorator(ICreateShipmentProcessor inner,ILogger<CargoValidationShipmentDecorator> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Shipment> HandleAsync(CreateShipmentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Validating cargo for shipment: CargoWeight={CargoWeight}, Origin={Origin}, Destination={Destination}",
            command.CargoWeight, command.Origin, command.Destination);

        Validate(command);

        _logger.LogDebug("Cargo validation passed: CargoWeight={CargoWeight}", command.CargoWeight);

        return await _inner.HandleAsync(command, cancellationToken);
    }

    private void Validate(CreateShipmentCommand command)
    {
        if (command.CargoWeight <= 0)
        {
            _logger.LogWarning("Validation failed: CargoWeight must be greater than zero. Received CargoWeight={CargoWeight}",
                command.CargoWeight);
            throw new ArgumentOutOfRangeException(nameof(command.CargoWeight), "Cargo weight must be greater than zero.");
        }

        if (command.CargoWeight > MaxCargoWeight)
        {
            _logger.LogWarning("Validation failed: CargoWeight exceeds maximum. Received CargoWeight={CargoWeight}, MaxCargoWeight={MaxCargoWeight}",
                command.CargoWeight, MaxCargoWeight);
            throw new ArgumentOutOfRangeException(nameof(command.CargoWeight),
                $"Cargo weight exceeds the allowed limit of {MaxCargoWeight} metric tons.");
        }

        if (command.CargoWeight > MaxCargoWeight * 0.5m)
        {
            _logger.LogWarning("Heavy cargo. CargoWeight={CargoWeight}", command.CargoWeight);
        }
    }
}


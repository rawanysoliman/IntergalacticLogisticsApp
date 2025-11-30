using IntergalacticLogistics.Application.Commands;
using IntergalacticLogistics.Domain;

namespace IntergalacticLogistics.Application.Interfaces;

public interface ICreateShipmentProcessor
{
    Task<Shipment> HandleAsync(CreateShipmentCommand command, CancellationToken cancellationToken = default);
}


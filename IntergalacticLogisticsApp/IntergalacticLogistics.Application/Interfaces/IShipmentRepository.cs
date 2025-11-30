using IntergalacticLogistics.Domain;

namespace IntergalacticLogistics.Application.Interfaces;

public interface IShipmentRepository
{
    Task<Shipment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Shipment shipment, CancellationToken cancellationToken = default);
    
}

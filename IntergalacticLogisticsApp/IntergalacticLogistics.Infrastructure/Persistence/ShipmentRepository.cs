using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Domain;
using Marten;

namespace IntergalacticLogistics.Infrastructure.Persistence;

public class ShipmentRepository : IShipmentRepository
{
    private readonly IDocumentSession _session;

    public ShipmentRepository(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<Shipment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _session.LoadAsync<Shipment>(id, cancellationToken);
    }

    public async Task<Guid> CreateAsync(Shipment shipment, CancellationToken cancellationToken = default)
    {
        _session.Store(shipment);
        await _session.SaveChangesAsync(cancellationToken);
        return shipment.Id;
    }


}


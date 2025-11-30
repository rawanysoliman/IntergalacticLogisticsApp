using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Domain;
using Wolverine;

namespace IntergalacticLogistics.Application.Queries;

public class GetShipmentQueryHandler
{
    private readonly IShipmentRepository _repository;

    public GetShipmentQueryHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Shipment?> Handle(GetShipmentQuery query, IMessageContext context)
    {

        return await _repository.GetByIdAsync(query.ShipmentId);
    }
}


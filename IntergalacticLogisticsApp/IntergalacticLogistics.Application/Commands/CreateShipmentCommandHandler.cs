using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Domain;
using Wolverine;

namespace IntergalacticLogistics.Application.Commands;

public class CreateShipmentCommandHandler
{
    private readonly ICreateShipmentProcessor _processor;

    public CreateShipmentCommandHandler(ICreateShipmentProcessor processor)
    {
        _processor = processor;
    }

    public async Task<Shipment> Handle(CreateShipmentCommand command, IMessageContext context)
    {
        return await _processor.HandleAsync(command);
    }
}


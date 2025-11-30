using IntergalacticLogistics.Application.Commands;
using IntergalacticLogistics.Application.Queries;
using IntergalacticLogistics.Domain;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace IntergalacticLogistics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public ShipmentsController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentCommand command)
    {
        var shipment = await _messageBus.InvokeAsync<Shipment>(command);
        return CreatedAtAction(nameof(GetShipment), new { id = shipment.Id }, shipment);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShipment(Guid id)
    {
        var query = new GetShipmentQuery(id);
        var result = await _messageBus.InvokeAsync<Shipment?>(query);
        return result != null ? Ok(result) : NotFound();
    }
}


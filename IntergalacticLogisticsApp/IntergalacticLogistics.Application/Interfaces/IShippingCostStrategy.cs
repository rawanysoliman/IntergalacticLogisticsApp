using IntergalacticLogistics.Domain;

namespace IntergalacticLogistics.Application.Interfaces;

public interface IShippingCostStrategy
{
    Task<decimal> CalculateCostAsync(Shipment shipment, CancellationToken cancellationToken = default);
    string StrategyName { get; }
}


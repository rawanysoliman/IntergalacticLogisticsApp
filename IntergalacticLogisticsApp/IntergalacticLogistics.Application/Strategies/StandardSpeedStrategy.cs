using IntergalacticLogistics.Application.Interfaces;
using IntergalacticLogistics.Domain;

namespace IntergalacticLogistics.Application.Strategies;

public class StandardSpeedStrategy : IShippingCostStrategy
{
    private readonly ISwapiClient _swapiClient;
    private const decimal BaseRatePerKg = 10.0m;
    private const decimal HyperdriveExtraFee = 50.0m;

    public StandardSpeedStrategy(ISwapiClient swapiClient)
    {
        _swapiClient = swapiClient;
    }

    public string StrategyName => "StandardSpeed";

    public async Task<decimal> CalculateCostAsync(Shipment shipment, CancellationToken cancellationToken = default)
    {
        var baseCost = shipment.CargoWeight * BaseRatePerKg;
        
        var starship = await _swapiClient.GetStarshipByIdAsync(shipment.StarshipId, cancellationToken);
        
        if (starship != null && 
            starship.HyperdriveRating != "n/a" &&
            decimal.TryParse(starship.HyperdriveRating, out var hyperdriveRating) &&
            hyperdriveRating > 2)
        {
            baseCost += HyperdriveExtraFee;
        }

        return Math.Round(baseCost, 2);
    }
}


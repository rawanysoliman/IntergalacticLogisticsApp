using IntergalacticLogistics.Application.Interfaces;

namespace IntergalacticLogistics.Application.Strategies;

public class HyperdriveExpressStrategy : IShippingCostStrategy
{
    private readonly ISwapiClient _swapiClient;
    private const decimal BaseRatePerKg = 20.0m;
    private const decimal HyperdriveExtraFee = 100.0m; 

    public HyperdriveExpressStrategy(ISwapiClient swapiClient)
    {
        _swapiClient = swapiClient;
    }

    public string StrategyName => "HyperdriveExpress";

    public async Task<decimal> CalculateCostAsync(Domain.Shipment shipment, CancellationToken cancellationToken = default)
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


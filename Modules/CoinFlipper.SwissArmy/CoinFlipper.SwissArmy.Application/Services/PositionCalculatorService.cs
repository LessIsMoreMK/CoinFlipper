using CoinFlipper.SwissArmy.Domain.Services;

namespace CoinFlipper.SwissArmy.Application.Services;

public class PositionCalculatorService : IPositionCalculatorService
{
    public (double initialMargin, double PNL, double ROE) 
        CalculatePositionStatsSimple(int leverage, bool isLong, double entryPrice, double exitPrice, double quantity)
    {
        var initialMargin = quantity * entryPrice / leverage;
        var PNL = quantity * (exitPrice - entryPrice);
        var ROE = (exitPrice - entryPrice) / entryPrice * 100 * leverage;

        PNL = Math.Round(PNL, 2);
        ROE = Math.Round(ROE, 2);

        // Adjust position direction
        if (!isLong)
        {
            PNL = -PNL;
            ROE = -ROE;
        }

        return (initialMargin, PNL, ROE);
    }
}
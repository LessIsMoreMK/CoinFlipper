namespace CoinFlipper.SwissArmy.Domain.Services;

public interface IPositionCalculatorService
{
    public (double initialMargin, double PNL, double ROE)
        CalculatePositionStatsSimple(int leverage, bool isLong, double entryPrice, double exitPrice, double quantity);
}
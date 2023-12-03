namespace CoinFlipper.SwissArmy.Application.Queries.PositionCalculator;

public class GetPositionStatsSimpleResponse(double initialMargin, double pnl, double roe)
{
    public double InitialMargin { get; set; } = initialMargin;

    public double PNL { get; set; } = pnl;

    public double ROE { get; set; } = roe;
}
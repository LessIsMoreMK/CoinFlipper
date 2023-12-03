using CoinFlipper.ServiceDefaults.Application.Queries;

namespace CoinFlipper.SwissArmy.Application.Queries.PositionCalculator;

public class GetPositionStatsSimpleRequest : IQuery<GetPositionStatsSimpleResponse>
{
    public int Leverage { get; set; } = 1;
    
    public bool IsLong { get; set; }
    
    public double EntryPrice { get; set; }
    
    public double ExitPrice { get; set; }
    
    public double Quantity { get; set; }
}
namespace CoinFlipper.Tracer.Domain.Services.Indicators;

public interface IRsiIndicatorService
{
    /// <summary>
    /// Calculates Relative Strength Index(RSI) for length of 14
    /// </summary>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol">coinSymbol</param>
    /// <returns>RSI value</returns>
    Task<decimal> CalculateRSI(Guid coinId, string coinSymbol);
}
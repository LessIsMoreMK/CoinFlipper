namespace CoinFlipper.Tracer.Domain.Services.Indicators;

public interface IMovingAverageIndicatorService
{
    /// <summary>
    /// Calculate Simple Moving Average(SMA)
    /// </summary>
    /// <param name="period">Period that SMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <returns>SMA value</returns>
    Task<decimal?> CalculateSMA(int period, Guid coinId, string coinSymbol);

    /// <summary>
    /// Calculate Exponential Moving Average(EMA)
    /// </summary>
    /// <param name="period">Period that EMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <returns>EMA value</returns>
    Task<decimal?> CalculateEMA(int period, Guid coinId, string coinSymbol);
    
    /// <summary>
    /// Calculate Volume Weighted Average Price(VWAP)
    /// </summary>
    /// <param name="period">Period that VWAP should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <returns>VWAP value</returns>
    Task<decimal?> CalculateVWAP(int period, Guid coinId, string coinSymbol);
    
    /// <summary>
    /// Calculate Smoothed Moving Average(SMMA)
    /// </summary>
    /// <param name="period">Period that SMMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <returns>SMMA value</returns>
    Task<decimal?> CalculateSMMA(int period, Guid coinId, string coinSymbol);
    
    /// <summary>
    /// Calculate Weighted Moving Average(WMA)
    /// </summary>
    /// <param name="period">Period that WMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <returns>WMA value</returns>
    Task<decimal?> CalculateWMA(int period, Guid coinId, string coinSymbol);
    
    /// <summary>
    /// Calculate Hull Moving Average(HMA)
    /// </summary>
    /// <param name="period">Period that HMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <returns>HMA value</returns>
    Task<decimal?> CalculateHMA(int period, Guid coinId, string coinSymbol);
}
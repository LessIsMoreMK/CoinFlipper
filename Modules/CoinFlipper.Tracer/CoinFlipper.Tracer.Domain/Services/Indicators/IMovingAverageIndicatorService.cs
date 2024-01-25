namespace CoinFlipper.Tracer.Domain.Services.Indicators;

public interface IMovingAverageIndicatorService
{
    /// <summary>
    /// Calculate Simple Moving Average(SMA)
    /// </summary>
    /// <param name="length">Period that SMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <param name="validateDateTime">True if checking records DateTime correctness should take place; false otherwise</param>
    /// <returns>SMA value; null when cannot calculate</returns>
    Task<decimal?> CalculateSMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true);

    /// <summary>
    /// Calculate Exponential Moving Average(EMA)
    /// </summary>
    /// <param name="length">Period that EMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <param name="validateDateTime">True if checking records DateTime correctness should take place; false otherwise</param>
    /// <returns>EMA value; null when cannot calculate</returns>
    Task<decimal?> CalculateEMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true);
    
    /// <summary>
    /// Calculate Volume Weighted Average Price(VWAP)
    /// </summary>
    /// <param name="length">Period that VWAP should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <param name="validateDateTime">True if checking records DateTime correctness should take place; false otherwise</param>
    /// <returns>VWAP value; null when cannot calculate</returns>
    Task<decimal?> CalculateVWAP(int length, Guid coinId, string coinSymbol, bool validateDateTime = true);
    
    /// <summary>
    /// Calculate Smoothed Moving Average(SMMA)
    /// </summary>
    /// <param name="length">Period that SMMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <param name="validateDateTime">True if checking records DateTime correctness should take place; false otherwise</param>
    /// <returns>SMMA value; null when cannot calculate</returns>
    Task<decimal?> CalculateSMMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true);
    
    /// <summary>
    /// Calculate Weighted Moving Average(WMA)
    /// </summary>
    /// <param name="length">Period that WMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <param name="validateDateTime">True if checking records DateTime correctness should take place; false otherwise</param>
    /// <returns>WMA value; null when cannot calculate</returns>
    Task<decimal?> CalculateWMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true);

    /// <summary>
    /// Calculate Hull Moving Average(HMA)
    /// </summary>
    /// <param name="length">Period that HMA should be calculated for</param>
    /// <param name="coinId">Application inner coinId</param>
    /// <param name="coinSymbol"></param>
    /// <param name="validateDateTime">True if checking records DateTime correctness should take place; false otherwise</param>
    /// <returns>HMA value; null when cannot calculate</returns>
    Task<decimal?> CalculateHMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true);
}
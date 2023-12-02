namespace CoinFlipper.Shared.DateTimeHelpers;

public static class DateTimeExtensions
{
    public static DateTime TimestampToDateTime(long timestamp, int baseYear = 1970)
    {
        var baseDate = baseYear switch
        {
            1970 => DateTime.UnixEpoch,
            2000 => new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            _ => throw new ArgumentException("Base year must be either 1970 or 2000.")
        };

        return baseDate.AddSeconds(timestamp);
    }
    
    public static DateTime Truncate(this System.DateTime value, DateTimePrecision precision)
        => new System.DateTime(value.Ticks - value.Ticks % (long) precision, value.Kind);
}
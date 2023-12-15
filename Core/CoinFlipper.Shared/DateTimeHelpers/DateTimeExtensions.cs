namespace CoinFlipper.Shared.DateTimeHelpers;

public static class DateTimeExtensions
{
    public static DateTime TimestampToDateTime(long timestamp, bool millisecond = false, int baseYear = 1970)
    {
        var baseDate = baseYear switch
        {
            1970 => DateTime.UnixEpoch,
            2000 => new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            _ => throw new ArgumentException("Base year must be either 1970 or 2000.")
        };
        
        return baseDate.AddSeconds(millisecond ? timestamp / 1000.0 : timestamp);
    }
    
    public static long DateTimeToTimestamp(DateTime dateTime, bool millisecond = false, int baseYear = 1970)
    {
        var baseDate = baseYear switch
        {
            1970 => DateTime.UnixEpoch,
            2000 => new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            _ => throw new ArgumentException("Base year must be either 1970 or 2000.")
        };

        if (dateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException("DateTime must be in UTC.");

        var totalSeconds = (long)(dateTime - baseDate).TotalSeconds;
        return millisecond ? totalSeconds * 1000 : totalSeconds;
    }
    
    public static DateTime Truncate(this DateTime value, DateTimePrecision precision)
        => new (value.Ticks - value.Ticks % (long) precision, value.Kind);
}